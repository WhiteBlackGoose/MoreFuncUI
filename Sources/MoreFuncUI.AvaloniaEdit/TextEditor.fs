module MoreFuncUI.AvaloniaEdit.TextEditor

open AvaloniaEdit
open Avalonia.FuncUI
open Avalonia.FuncUI.Builder
open Avalonia.FuncUI.Types
open AvaloniaEdit.Document
open AvaloniaEdit.Highlighting
open Avalonia.Media
open System.Text
open Avalonia.Input

type TextEditor with
    static member create(attrs : IAttr<TextEditor> list): IView<TextEditor> =
        ViewBuilder.Create<TextEditor>(attrs)

    static member document(doc : TextDocument) =
        AttrBuilder<TextEditor>.CreateProperty<TextDocument>(TextEditor.DocumentProperty, doc, ValueNone)

    static member options(opts : TextEditorOptions) =
        AttrBuilder<TextEditor>.CreateProperty<TextEditorOptions>(TextEditor.OptionsProperty, opts, ValueNone)

    static member syntaxHighlighting(definition : IHighlightingDefinition) =
        AttrBuilder<TextEditor>.CreateProperty<IHighlightingDefinition>(TextEditor.SyntaxHighlightingProperty, definition, ValueNone)

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




type TextEditor with
    static member onPreviewPointerHover(func: PointerEventArgs -> unit, ?subPatchOptions) =
        AttrBuilder<'t>.CreateSubscription<PointerEventArgs>(TextEditor.PreviewPointerHoverEvent, func, ?subPatchOptions = subPatchOptions)

    static member onPointerHover(func: PointerEventArgs -> unit, ?subPatchOptions) =
        AttrBuilder<'t>.CreateSubscription<PointerEventArgs>(TextEditor.PointerHoverEvent, func, ?subPatchOptions = subPatchOptions)

    static member onPreviewPointerHoverStopped(func: PointerEventArgs -> unit, ?subPatchOptions) =
        AttrBuilder<'t>.CreateSubscription<PointerEventArgs>(TextEditor.PreviewPointerHoverStoppedEvent, func, ?subPatchOptions = subPatchOptions)

    static member onPointerHoverStopped(func: PointerEventArgs -> unit, ?subPatchOptions) =
        AttrBuilder<'t>.CreateSubscription<PointerEventArgs>(TextEditor.PointerHoverStoppedEvent, func, ?subPatchOptions = subPatchOptions)