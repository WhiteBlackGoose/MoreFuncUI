module MoreFuncUI.AvaloniaEdit.TextEditor

open AvaloniaEdit
open AvaloniaEdit.TextMate
open Avalonia.FuncUI
open Avalonia.FuncUI.Builder
open Avalonia.FuncUI.Types
open AvaloniaEdit.Document
open AvaloniaEdit.Highlighting
open Avalonia.Media
open System.Text
open Avalonia.Input
open System.Runtime.CompilerServices

type ConditionalWeakTable<'TKey, 'TValue when 'TKey : not struct and 'TValue : not struct> with
    member this.AddOrUpdate(key : 'TKey, value : 'TValue) =
        match this.TryGetValue(key) with
        | (true, _) ->
            this.Remove(key) |> ignore
            this.Add(key, value)
        | _ ->
            this.Add(key, value)

type private Box<'t> = { Value : 't }

type private CWTHolder() =
    static let setLangIds = ConditionalWeakTable<TextEditor, string Option * TextMate.Installation>()
    static let themes = ConditionalWeakTable<TextEditor, Box<ThemeName>>()

    static member SetLangIds = setLangIds
    static member Themes = themes

type TextEditor with
    static member create(attrs : IAttr<TextEditor> list): IView<TextEditor> =
        ViewBuilder.Create<TextEditor>(attrs)

    static member document(doc : TextDocument) =
        AttrBuilder<TextEditor>.CreateProperty<TextDocument>(TextEditor.DocumentProperty, doc, ValueNone)

    static member options(opts : TextEditorOptions) =
        AttrBuilder<TextEditor>.CreateProperty<TextEditorOptions>(TextEditor.OptionsProperty, opts, ValueNone)

    static member wordWrap(wrap : bool) =
        AttrBuilder<TextEditor>.CreateProperty<bool>(TextEditor.WordWrapProperty, wrap, ValueNone)

    static member isReadOnly(is : bool) =
        AttrBuilder<TextEditor>.CreateProperty<bool>(TextEditor.IsReadOnlyProperty, is, ValueNone)

    static member isModified(is : bool) =
        AttrBuilder<TextEditor>.CreateProperty<bool>(TextEditor.IsModifiedProperty, is, ValueNone)

    static member showLineNumbers(is : bool) =
        AttrBuilder<TextEditor>.CreateProperty<bool>(TextEditor.ShowLineNumbersProperty, is, ValueNone)

    static member lineNumbersForeground(is : IBrush) =
        AttrBuilder<TextEditor>.CreateProperty<IBrush>(TextEditor.LineNumbersForegroundProperty , is, ValueNone)

    static member encoding(enc : Encoding) =
        AttrBuilder<TextEditor>.CreateProperty<Encoding>(TextEditor.EncodingProperty, enc, ValueNone)

    static member syntaxHighlightingLanguage(langId : string Option) =
        AttrBuilder<TextEditor>.CreateProperty<string Option>(
            "syntaxHighlightingLanguage",
            langId, 
            (fun editor -> 
                match CWTHolder.SetLangIds.TryGetValue(editor) with
                | (true, (value, _)) -> value
                | (false, _) -> None
                ) |> ValueSome, 

            (fun (editor, newValue) -> 
                let textMate =
                    match CWTHolder.SetLangIds.TryGetValue(editor) with
                    | (true, (_, textMate)) ->
                        CWTHolder.SetLangIds.AddOrUpdate(editor, (newValue, textMate))
                        textMate
                    | _ ->
                        let theme = CWTHolder.Themes.GetValue(editor, fun _ -> { Value = ThemeName.DarkPlus })
                        let textMate = editor.InstallTextMate(theme.Value)
                        CWTHolder.SetLangIds.AddOrUpdate(editor, (newValue, textMate))
                        textMate
                match newValue with
                | Some langId -> textMate.SetGrammarByLanguageId(langId)
                | None -> textMate.SetGrammar(null)
            ) |> ValueSome, ValueNone, fun () -> None)

    static member syntaxHighlightingTheme(theme : ThemeName) =
        AttrBuilder<TextEditor>.CreateProperty<ThemeName>(
            "syntaxHighlightingTheme",

           theme,

           (fun editor -> CWTHolder.Themes.GetValue(editor, fun _ -> { Value = ThemeName.DarkPlus }).Value)
           |> ValueSome,

           (fun (editor, newValue) ->

            CWTHolder.Themes.AddOrUpdate(editor, { Value = newValue })

            match CWTHolder.SetLangIds.TryGetValue(editor) with
            | (true, (_, installation)) ->
                installation.SetTheme(newValue)
            | _ -> ()

            )|> ValueSome,

           ValueNone,

           fun () -> ThemeName.DarkPlus
        )


type TextEditor with
    static member onPreviewPointerHover(func: PointerEventArgs -> unit, ?subPatchOptions) =
        AttrBuilder<'t>.CreateSubscription<PointerEventArgs>(TextEditor.PreviewPointerHoverEvent, func, ?subPatchOptions = subPatchOptions)

    static member onPointerHover(func: PointerEventArgs -> unit, ?subPatchOptions) =
        AttrBuilder<'t>.CreateSubscription<PointerEventArgs>(TextEditor.PointerHoverEvent, func, ?subPatchOptions = subPatchOptions)

    static member onPreviewPointerHoverStopped(func: PointerEventArgs -> unit, ?subPatchOptions) =
        AttrBuilder<'t>.CreateSubscription<PointerEventArgs>(TextEditor.PreviewPointerHoverStoppedEvent, func, ?subPatchOptions = subPatchOptions)

    static member onPointerHoverStopped(func: PointerEventArgs -> unit, ?subPatchOptions) =
        AttrBuilder<'t>.CreateSubscription<PointerEventArgs>(TextEditor.PointerHoverStoppedEvent, func, ?subPatchOptions = subPatchOptions)