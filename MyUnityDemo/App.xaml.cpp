//
// App.xaml.cpp
// Implementation of the App class.
//

#include "pch.h"
#include "MainPage.xaml.h"

using namespace Template;

using namespace Platform;
using namespace Windows::ApplicationModel;
using namespace Windows::ApplicationModel::Activation;
using namespace Windows::Foundation;
using namespace Windows::Foundation::Collections;
using namespace Windows::UI::Xaml;
using namespace Windows::UI::Xaml::Controls;
using namespace Windows::UI::Xaml::Controls::Primitives;
using namespace Windows::UI::Xaml::Data;
using namespace Windows::UI::Xaml::Input;
using namespace Windows::UI::Xaml::Interop;
using namespace Windows::UI::Xaml::Media;
using namespace Windows::UI::Xaml::Navigation;
using namespace Windows::UI::Core;
using namespace UnityPlayer;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

/// <summary>
/// Initializes the singleton application object.  This is the first line of authored code
/// executed, and as such is the logical equivalent of main() or WinMain().
/// </summary>
App::App()
{
	m_AppCallbacks = ref new AppCallbacks(false);
	m_AppCallbacks->RenderingStarted += ref new RenderingStartedHandler(this, &App::RemoveSplashScreen);
	InitializeComponent();
}


/// <summary>
/// Invoked when application is launched through protocol.
/// Read more - http://msdn.microsoft.com/library/windows/apps/br224742
/// </summary>
/// <param name="args"></param>
void App::OnActivated(Windows::ApplicationModel::Activation::IActivatedEventArgs^ pArgs)
{
	String^ appArgs = "";
	SplashScreen^ splashScreen = nullptr;
	switch (pArgs->Kind)
	{
	case ActivationKind::Protocol:
		ProtocolActivatedEventArgs^ eventArgs = dynamic_cast<ProtocolActivatedEventArgs^>(pArgs);
		splashScreen = eventArgs->SplashScreen;
		appArgs += "Uri=" + eventArgs->Uri->AbsoluteUri;
		break;
	}
	InitializeUnity(appArgs, splashScreen);
}

/// <summary>
/// Invoked when the application is launched normally by the end user.	Other entry points
/// will be used such as when the application is launched to open a specific file.
/// </summary>
/// <param name="e">Details about the launch request and process.</param>
void App::OnLaunched(Windows::ApplicationModel::Activation::LaunchActivatedEventArgs^ pArgs)
{
	// Do not repeat app initialization when already running, just ensure that
	// the window is active
	if (pArgs->PreviousExecutionState == ApplicationExecutionState::Running)
	{
		Window::Current->Activate();
		return;
	}

	if (pArgs->PreviousExecutionState == ApplicationExecutionState::Terminated)
	{
		//TODO: Load state from previously suspended application
	}

	InitializeUnity(pArgs->Arguments, pArgs->SplashScreen);
}

#if UNITY_WP_8_1
static void SetupLocationService(UnityPlayer::AppCallbacks^ appCallbacks);
#endif

void App::InitializeUnity(Platform::String^ args, Windows::ApplicationModel::Activation::SplashScreen^ splashScreen)
{
#if UNITY_WP_8_1
	Windows::UI::ViewManagement::ApplicationView::GetForCurrentView()->SuppressSystemOverlays = true;
	Windows::UI::ViewManagement::StatusBar::GetForCurrentView()->HideAsync();
#endif

	m_AppCallbacks->SetAppArguments(args);
	if (m_AppCallbacks->IsInitialized()) return;

	auto mainPage = ref new MainPage(splashScreen);
	Window::Current->Content = mainPage;
	Window::Current->Activate();

	// Setup scripting bridge
	_bridge = ref new WinRTBridge::WinRTBridge();
	m_AppCallbacks->SetBridge(_bridge);

#if !UNITY_WP_8_1
	m_AppCallbacks->SetKeyboardTriggerControl(mainPage);
#endif

	m_AppCallbacks->SetSwapChainPanel(mainPage->GetSwapChainPanel());

	auto coreWindow = Window::Current->CoreWindow;
	m_AppCallbacks->SetCoreWindowEvents(coreWindow);
	m_AppCallbacks->InitializeD3DXAML();
	
#if UNITY_WP_8_1
	SetupLocationService(m_AppCallbacks);
#endif
}

void App::RemoveSplashScreen()
{
	// This will fail if you change main window class
	// Make sure to adjust accordingly if you do something like this
	MainPage^ page = safe_cast<MainPage^>(Window::Current->Content);
	page->RemoveSplashScreen();
}

#if UNITY_WP_8_1
// This is the default setup to show location consent message box to the user
// You can customize it to your needs, but do not remove it completely if your application
// uses location services, as it is a requirement in Windows Store certification process
static void SetupLocationService(UnityPlayer::AppCallbacks^ appCallbacks)
{
	using namespace Concurrency;
	using namespace Windows::Storage;
	using namespace Windows::UI::Popups;

	if (!appCallbacks->IsLocationCapabilitySet())
	{
		return;
	}

	const auto settingName = ref new Platform::String(L"LocationContent");
	bool userGaveConsent = false;

	auto settings = ApplicationData::Current->LocalSettings;
	auto consent = settings->Values->Lookup(settingName);
	
	auto finishSetupingLocationService = [appCallbacks](bool userGaveConsent)
	{
		if (userGaveConsent)
		{	// Must be called from UI thread
			appCallbacks->SetupGeolocator();
		}
	};

	if (consent == nullptr)
	{
		auto messageDialog = ref new MessageDialog(L"Can this application use your location?", L"Location services");

		auto acceptCommand = ref new UICommand(L"Yes");
		auto declineCommand = ref new UICommand(L"No");

		messageDialog->Commands->Append(acceptCommand);
		messageDialog->Commands->Append(declineCommand);

		create_task(messageDialog->ShowAsync()).then([settings, acceptCommand, finishSetupingLocationService, settingName](task<IUICommand^> dialogTask)
		{
			auto userGaveConsent = dialogTask.get() == acceptCommand;
			settings->Values->Insert(settingName, userGaveConsent);
			finishSetupingLocationService(userGaveConsent);
		});
	}
	else
	{
		finishSetupingLocationService(safe_cast<bool>(consent));
	}
}
#endif
