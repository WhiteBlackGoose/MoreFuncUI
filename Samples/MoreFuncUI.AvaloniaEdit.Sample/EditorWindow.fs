namespace MoreFuncUI.AvaloniaEdit.Sample

open MoreFuncUI.AvaloniaEdit.TextEditor
open AvaloniaEdit.Document
open AvaloniaEdit
open Avalonia.Media

module Edit =
    open Avalonia.Controls
    open Avalonia.FuncUI.DSL
    
    type State = { 
        document : TextDocument 
        fontSize : int
        }

    let init (window : Window) () =
        { 
            document = TextDocument("let a = 5\nprintfn $\"Hello, world{a * 3}!\"" + string('a', 554)) 
            fontSize = 28
        }
        
    type Msg =
        | Noop
        | FontSizeInc

    let update (msg: Msg) (state: State) : State =
        match msg with
        | Noop -> state
        | FontSizeInc -> { state with fontSize = state.fontSize + 1 }
    
    let view (state: State) (dispatch) =
        TextEditor.create [
            TextEditor.name "editor"
            TextEditor.dock Dock.Top
            TextEditor.document state.document
            TextEditor.fontFamily "monospace"
            TextEditor.fontSize state.fontSize
            TextEditor.fontWeight FontWeight.Light
            TextEditor.showLineNumbers true
            TextEditor.isReadOnly false
            TextEditor.minHeight 500
            TextEditor.background Brushes.Transparent
            TextEditor.onPointerHover (fun p -> dispatch FontSizeInc)
        ]