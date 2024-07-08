var translateTask;

document.addEventListener('selectionchange', (e) => {
    if (translateTask) {
        clearTimeout(translateTask);
    }
    translateTask = setTimeout(translateSelected,1000)
});

function translateSelected() {
    if (!window.location.href.endsWith("Translator")) return;
    const selection = window.getSelection();
    const text = document.getSelection().toString().trim();
    if (text.length > 0) {
        const langFrom = document.querySelector('select[name="lang-from"]').value;
        const langTo = document.querySelector('select[name="lang-to"]').value;
        fetch("/Home/TranslateApi", {
            method: 'POST',
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded'
            },
            body: `lang-from=${langFrom}&lang-to=${langTo}&original-text=${text}`

        }).then(r => r.json()).then(j => {
            console.log(j);
            var range = selection.getRangeAt(0),
                span = document.createElement('span');
            span.className = 'translated';
            range.extractContents();
            span.innerText = j.text;
            range.insertNode(span);
            selection.empty();
        });
        console.log(text);
    }
    translateTask = false;
}
document.addEventListener('DOMContentLoaded', () => {
    const authButton = document.getElementById("auth-button");
    if (authButton) authButton.addEventListener('click', authButtonClick);
});
function authButtonClick() {
    const authEmail = document.getElementById("auth-email");
    if (!authEmail) throw "Element '#auth-email' not found";
    const authPassword = document.getElementById("auth-password");
    if (!authPassword) throw "Element '#auth-password' not found";

    const email = authEmail.value.trim();
    const password = authPassword.value;
    const authWarning = document.getElementById("auth-warning")
    if (email == "") {
        authWarning.classList.remove('visually-hidden');
        authWarning.innerText = "Enter Email";
        return;
    }
    if (password == "") {
        authWarning.classList.remove('visually-hidden');
        authWarning.innerText = "Enter Password";
        return;
    }
    fetch(`/api/auth?email=${email}&password=${password}`)
        .then(r => r.json())
        .then(j => {
            console.log(j);
            if (j.status == "success") {
                window.location.reload();
            }
            else {
                authWarning.classList.remove('visually-hidden');
                authWarning.innerText = "error enter";
            }
        });
}