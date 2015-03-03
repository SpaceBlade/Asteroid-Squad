//
// App.xaml.h
// Declaration of the App class.
//

#pragma once

#include "App.g.h"

namespace Template
{
	/// <summary>
	/// Provides application-specific behavior to supplement the default Application class.
	/// </summary>
	ref class App sealed
	{
	public:
		App();
		virtual void OnLaunched(Windows::ApplicationModel::Activation::LaunchActivatedEventArgs^ pArgs) override;
		virtual void OnActivated(Windows::ApplicationModel::Activation::IActivatedEventArgs^ pArgs) override;
	private:
		void InitializeUnity(Platform::String^ args, Windows::ApplicationModel::Activation::SplashScreen^ splashScreen);
		void RemoveSplashScreen();

		UnityPlayer::AppCallbacks^ m_AppCallbacks;

		WinRTBridge::WinRTBridge^ _bridge;
	};
}
