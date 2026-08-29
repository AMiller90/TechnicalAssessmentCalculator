window.calculatorKeyboard = {
    initialize: function (dotNetReference) {
        document.addEventListener("keydown", function (event) {
            const key = event.key.length === 1
                ? event.key.toUpperCase()
                : event.key;

            const handledKeys = [
                "0", "1", "2", "3", "4", "5",
                "6", "7", "8", "9",
                ".",
                "+", "-", "*", "/",
                "Enter", "=",
                "Escape",
                "Backspace",
                "%",
                "F9",
                "Q",
                "R",
                "@",
                "Delete"
            ];

            if (!handledKeys.includes(key)) {
                return;
            }

            event.preventDefault();

            dotNetReference.invokeMethodAsync("HandleKeyboardInput", key);
        });
    }
};
