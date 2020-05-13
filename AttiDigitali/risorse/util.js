function KeyDownHandler(event, btn) {
    // process only the Enter key
    if (event.keyCode == 13) {
        // cancel the default submit
        event.returnValue = false;
        event.cancel = true;
        // submit the form by programmatically clicking the specified button
        btn.click();
    }
}
