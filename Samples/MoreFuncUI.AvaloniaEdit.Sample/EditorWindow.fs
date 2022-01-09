namespace MoreFuncUI.AvaloniaEdit.Sample

open MoreFuncUI.AvaloniaEdit.TextEditor
open AvaloniaEdit.Document
open AvaloniaEdit
open Avalonia.Media
open AvaloniaEdit.TextMate
open Elmish

module Edit =
    open Avalonia.Controls
    open Avalonia.FuncUI.DSL
    
    type State = { 
        document1 : TextDocument 
        document2 : TextDocument 
        fontSize : int
        theme : ThemeName
        }

       
    type Msg =
        | Noop
        | ToLightScheme
        | ToDarkScheme

    let init () =
        { 
            document1 = TextDocument("""
let rec parseInner s : Result<Expression, string> =
    match s with
    | [] -> Error "Empty input"

    | [ ValidVariable x ] -> Ok (Variable x)

    | other ->
        let rec blockApplier applied blocks =
            match blocks with
            | [] -> applied |> Ok
            | hd::rest -> blockApplier (Applied(applied, hd)) rest

        opt {
            let! blocks = blocksParse other
            let res = 
                match blocks with
                | hd::tl -> blockApplier hd tl
                | _ -> Error "Empty block"
            return! res
        }

and blocksParse s : Result<Expression list, string> =
    match s with
    | ValidVariable x::rest ->
        blocksParse rest
        |> Result.map (fun parsed -> (Variable x) :: parsed)

    | '\\'::rest ->
        opt {
            let! (lambda, other) = parseLambda rest
            let! parsed = blocksParse other
            return lambda :: parsed
        }

    | '('::rest ->
        opt {
            let! (left, other) = untilCan rest
            let! parsed = parseInner left
            let! blocks = blocksParse other
            return parsed :: blocks
        }

    | [] -> Ok []

    | err -> Error $"Unexpected block start {err}"

and parseLambda s : Result<Expression * char list, string> =
    match s with
    | ValidVariable x::rest ->
        opt {
            let! (lambda, other) = parseLambda rest
            return Lambda(x, lambda), other
        }

    | '.'::rest ->
        opt {
            let! (expr, other) = untilCan rest
            let! parsed = parseInner expr
            return parsed, other
        }

    | '.'::ValidVariable x::rest -> parseFlat (x::rest)
        
    | err -> Error $"Unexpected syntax for lambda: {err}"

and parseFlat s : Result<Expression * char list, string> =
    match s with
    | [ ValidVariable x ] -> (Variable x, []) |> Ok

    | ValidVariable x::'('::rest -> (Variable x, '('::rest) |> Ok

    | ValidVariable x::'\\'::rest -> (Variable x, '\\'::rest) |> Ok

    | ValidVariable x::other ->
        let (variableLine, other) = untilNotVariable other
        let rec variableLineApply (s : char list) res =
            match s with
            | x::rest ->
                variableLineApply rest (Applied(res, Variable x))
            | [] ->
                res
        (variableLineApply variableLine (Variable x), other)
        |> Ok

    | err -> Error $"Flat parser encountered unexpected: {err}"
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
            fontSize = 28;
            theme = ThemeName.DarkPlus
        }
        
    

    let update (msg: Msg) (state: State) =
        match msg with
        | Noop -> state
        | ToLightScheme -> { state with theme = ThemeName.Light }
        | ToDarkScheme -> { state with theme = ThemeName.KimbieDark }
    

    let view (state: State) (dispatch) =
        DockPanel.create [
            DockPanel.name "panel"
            DockPanel.children [
                TextEditor.create [
                    TextEditor.dock Dock.Left
                    TextEditor.document state.document1
                    TextEditor.fontFamily "monospace"
                    TextEditor.fontSize state.fontSize
                    TextEditor.fontWeight FontWeight.Light
                    TextEditor.showLineNumbers true
                    TextEditor.background Brushes.Transparent
                    TextEditor.syntaxHighlightingLanguage (Some "fsharp")
                    TextEditor.syntaxHighlightingTheme state.theme
                ]
                TextEditor.create [
                    TextEditor.dock Dock.Left
                    TextEditor.document state.document2
                    TextEditor.fontFamily "arial"
                    TextEditor.fontSize state.fontSize
                    TextEditor.fontWeight FontWeight.Light
                    TextEditor.fontStyle FontStyle.Italic
                    TextEditor.showLineNumbers true
                    TextEditor.isReadOnly false
                    TextEditor.minHeight 500
                    TextEditor.background Brushes.White
                    TextEditor.foreground Brushes.Brown
                ]
                Button.create [
                    Button.content "ToLight"
                    Button.onClick (fun _ -> dispatch ToLightScheme )
                ]
                Button.create [
                    Button.content "ToDark"
                    Button.onClick (fun _ -> dispatch ToDarkScheme )
                ]
            ]
        ]