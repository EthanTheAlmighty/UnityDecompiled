using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
	/// <summary>
	///   <para>Controls the global audio settings from script.</para>
	/// </summary>
	public sealed class AudioSettings
	{
		/// <summary>
		///   <para>A delegate called whenever the global audio settings are changed, either by AudioSettings.Reset or by an external device change such as the OS control panel changing the sample rate or because the default output device was changed, for example when plugging in an HDMI monitor or a USB headset.</para>
		/// </summary>
		/// <param name="deviceWasChanged">True if the change was caused by an device change.</param>
		public delegate void AudioConfigurationChangeHandler(bool deviceWasChanged);

		public static event AudioSettings.AudioConfigurationChangeHandler OnAudioConfigurationChanged
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				AudioSettings.OnAudioConfigurationChanged = (AudioSettings.AudioConfigurationChangeHandler)Delegate.Combine(AudioSettings.OnAudioConfigurationChanged, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				AudioSettings.OnAudioConfigurationChanged = (AudioSettings.AudioConfigurationChangeHandler)Delegate.Remove(AudioSettings.OnAudioConfigurationChanged, value);
			}
		}

		/// <summary>
		///   <para>Returns the speaker mode capability of the current audio driver. (Read Only)</para>
		/// </summary>
		public static extern AudioSpeakerMode driverCapabilities
		{
			[WrapperlessIcall]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		/// <summary>
		///   <para>Gets the current speaker mode. Default is 2 channel stereo.</para>
		/// </summary>
		public static extern AudioSpeakerMode speakerMode
		{
			[WrapperlessIcall]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[WrapperlessIcall]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		/// <summary>
		///   <para>Returns the current time of the audio system.</para>
		/// </summary>
		public static extern double dspTime
		{
			[WrapperlessIcall]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		/// <summary>
		///   <para>Get the mixer's current output rate.</para>
		/// </summary>
		public static extern int outputSampleRate
		{
			[WrapperlessIcall]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[WrapperlessIcall]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		internal static extern bool editingInPlaymode
		{
			[WrapperlessIcall]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[WrapperlessIcall]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		[EditorBrowsable(EditorBrowsableState.Never), Obsolete("AudioSettings.driverCaps is obsolete. Use driverCapabilities instead (UnityUpgradable) -> driverCapabilities", true)]
		public static AudioSpeakerMode driverCaps
		{
			get
			{
				return AudioSettings.driverCapabilities;
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void GetDSPBufferSize(out int bufferLength, out int numBuffers);

		[Obsolete("AudioSettings.SetDSPBufferSize is deprecated and has been replaced by audio project settings and the AudioSettings.GetConfiguration/AudioSettings.Reset API."), WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetDSPBufferSize(int bufferLength, int numBuffers);

		/// <summary>
		///   <para>Returns the current configuration of the audio device and system. The values in the struct may then be modified and reapplied via AudioSettings.Reset.</para>
		/// </summary>
		/// <returns>
		///   <para>The new configuration to be applied.</para>
		/// </returns>
		public static AudioConfiguration GetConfiguration()
		{
			AudioConfiguration result;
			AudioSettings.INTERNAL_CALL_GetConfiguration(out result);
			return result;
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_GetConfiguration(out AudioConfiguration value);

		/// <summary>
		///   <para>Performs a change of the device configuration. In response to this the AudioSettings.OnAudioConfigurationChanged delegate is invoked with the argument deviceWasChanged=false. It cannot be guaranteed that the exact settings specified can be used, but the an attempt is made to use the closest match supported by the system.</para>
		/// </summary>
		/// <param name="config">The new configuration to be used.</param>
		/// <returns>
		///   <para>True if all settings could be successfully applied.</para>
		/// </returns>
		public static bool Reset(AudioConfiguration config)
		{
			return AudioSettings.INTERNAL_CALL_Reset(ref config);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool INTERNAL_CALL_Reset(ref AudioConfiguration config);

		[RequiredByNativeCode]
		internal static void InvokeOnAudioConfigurationChanged(bool deviceWasChanged)
		{
			if (AudioSettings.OnAudioConfigurationChanged != null)
			{
				AudioSettings.OnAudioConfigurationChanged(deviceWasChanged);
			}
		}
	}
}
