﻿using Xamarin.Forms;
using System.Threading.Tasks;
using Android.Widget;

namespace Xamarin.Platform.Handlers.DeviceTests
{
	public partial class SliderHandlerTests
	{
		SeekBar GetNativeSlider(SliderHandler sliderHandler) =>
			((SeekBar)sliderHandler.View);

		double GetNativeProgress(SliderHandler sliderHandler) =>
			GetNativeSlider(sliderHandler).Progress;

		double GetNativeMaximum(SliderHandler sliderHandler) =>
			GetNativeSlider(sliderHandler).Max;

		Task ValidateNativeThumbColor(ISlider slider, Color color)
		{
			return InvokeOnMainThreadAsync(() =>
			{
				GetNativeSlider(CreateHandler(slider)).AssertContainsColor(color);
			});
		}
	}
}