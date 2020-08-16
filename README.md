# Rotrics.Net

SDK for controlling Rotrics robot arm.

## Usage

Available via NuGet [here](https://www.nuget.org/packages/Rotrics.Net/).

Call `ControllerFactory.GetController()` to get a configured instance of the controller. The controller implements `IDisposable`, so either use within a `using` statement as below or manually call `.Dispose()` when finished with it.

Be sure to call the methods `.Connect()` and `.MoveToHome()` before anything else.

So, to get up and running, it's basically:

```C#
using (var controller = ControllerFactory.GetController())
{
	controller.Connect(); // Will throw RotricsConnectionException if unable to connect.

	controller.MoveToHome();

	// Start controlling the arm! Just use IntelliSense to see what methods are currently available.
}
```

## Notes

- At Y300, Z0, X seems to be safe for -250 to 250.
- At X0, Z0, Y seems to be safe for 180 to 390.
- At X0, Y300, Z seems to be safe for -110 to 160.

## Useful links

- [Reachable positions](https://github.com/Rotrics-Dev/Marlin_For_DexArm/blob/DexArm_Dev/Marlin/src/module/dexarm/dexarm_position_reachable.h)