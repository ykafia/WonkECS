namespace SoftTouch.ECS.Scheduling2;


#region Startup
/// <summary>
/// Runs once before the startup stage.
/// </summary>
public sealed record PreStartup : SubStage<Main>;
/// <summary>
/// Runs once at the start of the app.
/// </summary>
public sealed record Startup : SubStage<Main>;
/// <summary>
/// Runs once after the startup stage.
/// </summary>
public sealed record PostStartup : SubStage<Main>;
#endregion

#region Main
/// <summary>
/// Runs first before everything else.
/// </summary>
public sealed record First : SubStage<Main>;
/// <summary>
/// Runs before the Update stage.
/// </summary>
public sealed record PreUpdate : SubStage<Main>;
/// <summary>
/// Runs everytime, contains the main logic.
/// </summary>
public sealed record Update : SubStage<Main>;
/// <summary>
/// Runs after the Update stage.
/// </summary>
public sealed record PostUpdate : SubStage<Main>;
/// <summary>
/// Runs last after everything else.
/// </summary>
public sealed record Last : SubStage<Main>;
#endregion