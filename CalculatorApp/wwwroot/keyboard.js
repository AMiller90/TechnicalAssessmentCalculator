window.calculatorKeyboard = {
    initialize: function (dotNetReference) {
        document.addEventListener("keydown", function (event) {
            const handledKeys = [
                "0", "1", "2", "3", "4", "5",
                "6", "7", "8", "9",
                ".",
                "+", "-", "*", "/",
                "Enter", "=",
                "Escape",
                "Backspace",
                "%"
            ];

            if (!handledKeys.includes(event.key)) {
                return;
            }

            event.preventDefault();

            dotNetReference.invokeMethodAsync("HandleKeyboardInput", event.key);
        });
    }
};
