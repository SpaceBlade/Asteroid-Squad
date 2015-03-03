//
// MainPage.xaml.h
// Declaration of the MainPage class.
//

#pragma once

#include "MainPage.g.h"

namespace Template
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	[Windows::Foundation::Metadata::WebHostHidden]
	public ref class MainPage sealed
	{
	public:
		MainPage(Windows::ApplicationModel::Activation::SplashScreen^ splashScreen);

		Windows::UI::Xaml::Controls::SwapChainPanel^ GetSwapChainPanel() { return DXSwapChainPanel; };
		void RemoveSplashScreen();
	protected:
		virtual void OnNavigatedTo(Windows::UI::Xaml::Navigation::NavigationEventArgs^ e) override;

#if !UNITY_WP_8_1
		virtual Windows::UI::Xaml::Automation::Peers::AutomationPeer^ OnCreateAutomationPeer() override;
#endif

	private:
		void OnResize();
		void PositionImage();
		void GetSplashBackgroundColor();
		std::wstring ExtractSplashBackgroundColor(const std::wstring&);

		Windows::ApplicationModel::Activation::SplashScreen^ m_Splash;
		Windows::Foundation::Rect m_SplashImageRect;
		Windows::Foundation::EventRegistrationToken m_OnResizeToken;
	};
}
