﻿namespace Xamarin.Platform.Handlers.DeviceTests.Stubs
{
	public partial class StepperStub : StubBase, IStepper
	{
		public double Increment { get; set; }

		public double Minimum { get; set; }

		public double Maximum { get; set; }

		public double Value { get; set; }

		public void ValueChanged() { }
	}
}