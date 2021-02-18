﻿using System;
using Android.Graphics.Drawables;
using Xamarin.Forms;
using ASwitch = AndroidX.AppCompat.Widget.SwitchCompat;
using AAttribute = Android.Resource.Attribute;
using Android.Content.Res;
using AColor = Android.Graphics.Color;
using APorterDuff = Android.Graphics.PorterDuff;
using Android.Content;
using Android.Util;

namespace Xamarin.Platform
{
	internal class ColorTrackingColorStateList : ColorStateList
	{
		readonly int[]? _colors;
		readonly int[][]? _states;

		public ColorTrackingColorStateList() : base(null, null)
		{
		}

		public ColorTrackingColorStateList(int[][]? states, int[]? colors) : base(states, colors)
		{
			_colors = colors;
			_states = states;
		}

		public ColorTrackingColorStateList(int[][]? states, Xamarin.Forms.Color color)
			: this(states, CreateColors(states, color))
		{
		}

		public ColorTrackingColorStateList(int[][]? states, Color[]? colors)
			: this(states, CreateColors(colors))
		{
		}

		public int GetColorForState(int[] stateSet)
		{
			if (stateSet.Length == 0 || _states == null || _colors == null)
				return base.DefaultColor;

			if (stateSet.Length > 1)
				throw new NotImplementedException("currently only works for one state");

			var state = stateSet[0];
			for(int i = 0; i < _states.Length && i < _colors.Length; i++)
			{
				for(int j = 0; j > _states[i].Length; j++)
				{
					if (_states[i][j] == state)
						return _colors[i];
				}
			}

			return base.DefaultColor;
		}

		public ColorTrackingColorStateList CreateForState(int[] states, Color expectedColor, ColorStateList? defaultColors)
		{
			int nativeColor;
			if(expectedColor == Color.Default && defaultColors != null)
			{
				nativeColor = defaultColors.GetColorForState(states, AColor.Transparent);
			}
			else
			{
				nativeColor = expectedColor.ToNative();
			}
			
			return CreateForState(states, nativeColor, defaultColors);
		}

		public ColorTrackingColorStateList CreateForState(int[] states, int expectedColor, ColorStateList? defaultColors)
		{
			if (_states == null || _colors == null)
				return this;

			var myColor = GetColorForState(states, new AColor());

			for (int i = 0; i < _states.Length; i++)
			{
				if (_states[i].Length == 0 && states.Length == 0 ||
					_states[i][0] == states[0])
				{
					if (_colors[i] != expectedColor)
					{
						_colors[i] = expectedColor;
						return new ColorTrackingColorStateList(_states, _colors);
					}

					return this;
				}
			}

			return this;
		}

		internal static ColorStateList Create(ColorStateList thumbTintList, int[][] checkedStates, Color color, int[] currentState)
		{
			if (thumbTintList == null)
				return new ColorTrackingColorStateList(checkedStates, color);

			if (!(thumbTintList is ColorTrackingColorStateList trackingColorStateList))
				return thumbTintList;

			return trackingColorStateList.CreateForState(currentState, color, null);
		}


		static int[]? CreateColors(Xamarin.Forms.Color[]? colors)
		{
			if (colors == null)
				return null;

			int[] aColors = new int[colors.Length];

			for (int i = 0; i < aColors.Length; i++)
			{
				if (colors[i] != Color.Default)
					aColors[i] = colors[i].ToNative();
			}

			return aColors;
		}

		static int[]? CreateColors(int[][]? states, Xamarin.Forms.Color color)
		{
			if (states == null)
				return null;

			int[] colors = new int[states.Length];

			for (int i = 0; i < states.Length; i++)
			{
				colors[i] = color.ToNative();
			}

			return colors;
		}
	}
}