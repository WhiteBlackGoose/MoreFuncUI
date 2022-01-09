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
@staticmethod
def prepare_ds(X, y, cfg, test=True):
    len1 = round(len(y) * (1 - cfg.VAL_SHARE))
    len2 = round(len(y) * cfg.VAL_SHARE / REVE)
    X_classes = Functional.get_x_classes(X, y, cfg.CLASS_COUNT)
    assert len(X_classes) == cfg.CLASS_COUNT, "An error occurred while get X_classes"
    if test:
        fX_train, fy_train = Functional.get_ds(X_classes, len1, cfg, True)
        fX_test, fy_test = Functional.get_ds(X_classes, len2, cfg, False)
        return fX_train, fy_train, fX_test, fy_test
    else:
        return Functional.get_ds(X_classes, len1 + len2, cfg, False)

@staticmethod
def gen_paths(path):
    directories = os.listdir(path)
    res = []
    for directory in directories:
        pp = path + "/" + directory
        files = os.listdir(pp)
        res.append([pp + "/" + i for i in files])
    return res
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
                    TextEditor.showLineNumbers true
                    TextEditor.background Brushes.Transparent
                    TextEditor.syntaxHighlightingLanguage (Some "fsharp")
                    TextEditor.syntaxHighlightingTheme ThemeName.DarkPlus
                ]
                TextEditor.create [
                    TextEditor.dock Dock.Left
                    TextEditor.document state.document1
                    TextEditor.fontFamily "monospace"
                    TextEditor.fontSize state.fontSize
                    TextEditor.showLineNumbers true
                    TextEditor.background Brushes.Transparent
                    TextEditor.syntaxHighlightingLanguage (Some "fsharp")
                    TextEditor.syntaxHighlightingTheme ThemeName.KimbieDark
                ]
                TextEditor.create [
                    TextEditor.dock Dock.Left
                    TextEditor.document state.document2
                    TextEditor.fontFamily "monospace"
                    TextEditor.fontSize state.fontSize
                    TextEditor.showLineNumbers true
                    TextEditor.background Brushes.Transparent
                    TextEditor.syntaxHighlightingLanguage (Some "python")
                    TextEditor.syntaxHighlightingTheme ThemeName.DimmedMonokai
                ]
            ]
        ]