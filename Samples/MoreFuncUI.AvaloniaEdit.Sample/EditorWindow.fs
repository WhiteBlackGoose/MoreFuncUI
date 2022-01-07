namespace MoreFuncUI.AvaloniaEdit.Sample

open MoreFuncUI.AvaloniaEdit.TextEditor
open AvaloniaEdit.Document
open AvaloniaEdit
open Avalonia.Media

module Edit =
    open Avalonia.Controls
    open Avalonia.FuncUI.DSL
    
    type State = { 
        document1 : TextDocument 
        document2 : TextDocument 
        fontSize : int
        }

    let init () =
        { 
            document1 = TextDocument("""let a = 5
printfn $"Hello, world{a * 3}!"
If you have never heard of F#, it is a general purpose 
functional/hybrid programming language which is great 
for tackling almost any kind of software challenge. F# 
is free and open source, and runs on Linux, Mac, 
Windows and more. Find out more at the F# Foundation.
""");
            document2 = TextDocument("""
“Thinking functionally” is critical to getting the most out of F#, 
so I will spend a lot of time on getting the basics down, and I 
will generally avoid too much discussion of the hybrid and OO features.

The site will mostly focus on mainstream business problems, such as 
domain driven design, website development, data processing, business 
rules, and so on. In the examples I will try to use business concepts 
such as Customer, Product, and Order, rather than overly academic ones.
""");
            fontSize = 28
        }
        
    type Msg =
        | Noop

    let update (msg: Msg) (state: State) : State =
        match msg with
        | Noop -> state
    
    let view (state: State) (dispatch) =
        DockPanel.create [
            DockPanel.children [
                TextEditor.create [
                    TextEditor.name "editor1"
                    TextEditor.dock Dock.Left
                    TextEditor.document state.document1
                    TextEditor.fontFamily "monospace"
                    TextEditor.fontSize state.fontSize
                    TextEditor.fontWeight FontWeight.Light
                    TextEditor.showLineNumbers true
                    TextEditor.isReadOnly false
                    TextEditor.minHeight 500
                    TextEditor.background Brushes.Transparent
                ]
                TextEditor.create [
                    TextEditor.name "editor2"
                    TextEditor.dock Dock.Left
                    TextEditor.document state.document2
                    TextEditor.fontFamily "arial"
                    TextEditor.fontSize state.fontSize
                    TextEditor.fontWeight FontWeight.Light
                    TextEditor.fontStyle FontStyle.Italic
                    TextEditor.showLineNumbers true
                    TextEditor.isReadOnly false
                    TextEditor.minHeight 500
                    TextEditor.background Brushes.Transparent
                ]
            ]
        ]