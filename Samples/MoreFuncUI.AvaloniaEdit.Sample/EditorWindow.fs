namespace MoreFuncUI.AvaloniaEdit.Sample

open MoreFuncUI.AvaloniaEdit
open AvaloniaEdit.Document
open AvaloniaEdit
open Avalonia.Media

module Edit =
    open Avalonia.Controls
    open Avalonia.FuncUI.DSL
    
    type State = { document : TextDocument }
    let init =
        { document = TextDocument("let a = 5\nprintfn $\"Hello, world{a * 3}!\"") }
        

    type Msg =
        | Noop

    let update (msg: Msg) (state: State) : State =
        match msg with
        | Noop -> state
    
    let view (state: State) (dispatch) =
        TextEditor.create [
            TextEditor.dock Dock.Top
            TextEditor.document state.document
            TextEditor.fontSize 14
            TextEditor.fontWeight FontWeight.Light
            TextEditor.showLineNumbers true
            TextEditor.isReadOnly false
        ]