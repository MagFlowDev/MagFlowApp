window.formPost = {
    post: function (url, fields) {
        const form = document.createElement("form");
        form.method = "POST";
        form.action = url;

        const token = document.querySelector('input[name="__RequestVerificationToken"]').value;
        fields["__RequestVerificationToken"] = token;
        for (const key in fields) {
            const input = document.createElement("input");
            input.type = "hidden";
            input.name = key;
            input.value = fields[key] ?? "";
            form.appendChild(input);
        }

        document.body.appendChild(form);
        form.submit();
    }
};