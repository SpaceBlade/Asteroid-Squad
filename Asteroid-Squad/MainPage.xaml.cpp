//
// MainPage.xaml.cpp
// Implementation of the MainPage class.
//

#include "pch.h"
#include "MainPage.xaml.h"
#include <ppltasks.h>
#include <string>

using namespace Template;

using namespace Platform;
using namespace Windows::ApplicationModel::Activation;
using namespace Windows::Foundation;
using namespace Windows::Foundation::Collections;
using namespace Windows::Storage;
using namespace Windows::UI;
using namespace Windows::UI::Core;
using namespace Windows::UI::Xaml;
using namespace Windows::UI::Xaml::Controls;
using namespace Windows::UI::Xaml::Controls::Primitives;
using namespace Windows::UI::Xaml::Data;
using namespace Windows::UI::Xaml::Input;
using namespace Windows::UI::Xaml::Media;
using namespace Windows::UI::Xaml::Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

MainPage::MainPage(SplashScreen^ splashScreen)
	: m_Splash(splashScreen)
{
	InitializeComponent();

	GetSplashBackgroundColor();
	OnResize();
	m_OnResizeToken = Window::Current->SizeChanged += ref new WindowSizeChangedEventHandler(
		[this] (Object^, WindowSizeChangedEventArgs^) { OnResize(); });
}

/// <summary>
/// Invoked when this page is about to be displayed in a Frame.
/// </summary>
/// <param name="e">Event data that describes how this page was reached.  The Parameter
/// property is typically used to configure the page.</param>
void MainPage::OnNavigatedTo(NavigationEventArgs^ e)
{
	m_Splash = static_cast<SplashScreen^>(e->Parameter);
	OnResize();
}

void MainPage::OnResize()
{
	if (m_Splash)
	{
		m_SplashImageRect = m_Splash->ImageLocation;
		PositionImage();
	}
}

void MainPage::PositionImage()
{
	auto inverseScaleX = 1.0f / DXSwapChainPanel->CompositionScaleX;
	auto inverseScaleY = 1.0f / DXSwapChainPanel->CompositionScaleY;

	ExtendedSplashImage->SetValue(Canvas::LeftProperty, m_SplashImageRect.X * inverseScaleX);
	ExtendedSplashImage->SetValue(Canvas::TopProperty, m_SplashImageRect.Y * inverseScaleY);
	ExtendedSplashImage->Height = m_SplashImageRect.Height * inverseScaleY;
	ExtendedSplashImage->Width = m_SplashImageRect.Width * inverseScaleX;
}

void MainPage::GetSplashBackgroundColor()
{
	auto storageTask = concurrency::create_task(StorageFile::GetFileFromApplicationUriAsync(
		ref new Windows::Foundation::Uri("ms-appx:///AppxManifest.xml")));
	storageTask.then([this] (StorageFile^ file)
	{
		auto readTask = concurrency::create_task(FileIO::ReadTextAsync(file));
		readTask.then([this] (String^ content)
		{
			try
			{
				std::wstring manifest(content->Data());
				manifest = ExtractSplashBackgroundColor(manifest);
				if (manifest != L"")
				{
					int32_t value = std::stoi(manifest, 0, 16) & 0x00FFFFFF;
					Color color;
					color.A = 0xFF;
					color.R = (byte) (value >> 16);
					color.G = (byte) ((value & 0x0000FF00) >> 8);
					color.B = (byte) (value & 0x000000FF);
					SolidColorBrush^ brush = ref new SolidColorBrush(color);

					CoreWindow::GetForCurrentThread()->Dispatcher->RunAsync(CoreDispatcherPriority::High,
						ref new DispatchedHandler([this, brush] { ExtendedSplashGrid->Background = brush; }));
				}
			}
			catch (std::exception&)
			{}
		});
	});
}

std::wstring MainPage::ExtractSplashBackgroundColor(const std::wstring& manifestXml)
{
	std::wstring manifest(manifestXml);
	std::wstring::size_type idx = manifest.find(L"SplashScreen");
	if (idx == std::wstring::npos)
		throw std::runtime_error("SplashScreen not found");
	manifest = manifest.substr(idx);
	idx = manifest.find(L"BackgroundColor");
	if (idx == std::wstring::npos)
		return L"";  // background is optional
	manifest = manifest.substr(idx);
	idx = manifest.find(L"\"");
	if (idx == std::wstring::npos || idx + 2 > manifest.length())
		throw std::runtime_error("Quote not found or incorrect string");
	manifest = manifest.substr(idx + 2); // remove quote and # after it
	idx = manifest.find(L"\"");
	if (idx == std::wstring::npos)
		throw std::runtime_error("Quote not found");
	manifest.erase(idx);
	return manifest;
}

void MainPage::RemoveSplashScreen()
{
	unsigned int idx;
	if (DXSwapChainPanel->Children->IndexOf(ExtendedSplashGrid, &idx))
		DXSwapChainPanel->Children->RemoveAt(idx);
	if (m_OnResizeToken.Value != 0)
	{
		Window::Current->SizeChanged -= m_OnResizeToken;
		m_OnResizeToken.Value = 0;
	}
}

#if !UNITY_WP_8_1
Windows::UI::Xaml::Automation::Peers::AutomationPeer^ MainPage::OnCreateAutomationPeer()
{
	return ref new UnityPlayer::XamlPageAutomationPeer(this);
}
#endif
