export function afterWebStarted(blazor) {
    blazor.registerCustomEventType('stocktakeeditorfocusout', {
        browserEventName: 'focusout',
        createEventArgs: event => ({
            staysInsideEditor: event.target.closest('[data-stocktake-editor]')
                ?.contains(event.relatedTarget) ?? false
        })
    });
}
