namespace MoreFuncUI.AvaloniaEdit.Sample

open Elmish
open Avalonia
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.Input
open Avalonia.FuncUI
open Avalonia.FuncUI.Elmish
open Avalonia.FuncUI.Components.Hosts
open Avalonia.Diagnostics
open Avalonia

type MainWindow() as this =
    inherit HostWindow()
    do
        // this.OnInitialized
        base.Title <- "MoreFuncUI.AvaloniaEdit.Sample"
        base.Width <- 1800.0
        base.Height <- 900.0

        this.AttachDevTools ()

        Elmish.Program.mkSimple Edit.init Edit.update Edit.view
        |> Program.withHost this
        |> Program.run

        
type App() =
    inherit Application()

    override this.Initialize() =
        this.Styles.Load "avares://Avalonia.Themes.Default/DefaultTheme.xaml"
        this.Styles.Load "avares://Avalonia.Themes.Default/Accents/BaseDark.xaml"
        this.Styles.Load "avares://AvaloniaEdit/AvaloniaEdit.xaml"

    override this.OnFrameworkInitializationCompleted() =
        match this.ApplicationLifetime with
        | :? IClassicDesktopStyleApplicationLifetime as desktopLifetime ->
            desktopLifetime.MainWindow <- MainWindow()
        | _ -> ()

module Program =

    [<EntryPoint>]
    let main(args: string[]) =
        AppBuilder
            .Configure<App>()
            .UsePlatformDetect()
            .UseSkia()
            .StartWithClassicDesktopLifetime(args)