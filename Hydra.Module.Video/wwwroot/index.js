const content = "/_content/";
const module = (document.getElementById("index--script").dataset.module || "");
const path = module ? `/${module}/` : "/";

const styles = [
    `${path}Hydra.Module.Video.styles.css`,
    `${path}css/app.css`,
    `${content}Blazorise.Bootstrap/blazorise.bootstrap.css`,
    `${content}Blazorise/blazorise.css`,
    `${content}Blazorise.Snackbar/blazorise.snackbar.css`,
    `${path}lib/font-awesome/css/all.min.css`,
    `${path}lib/bootstrap/css/bootstrap.min.css`
];

const scripts = [
    `${path}lib/jquery/dist/jquery.min.js`,
    `${path}lib/bootstrap/js/bootstrap.bundle.min.js`,
    `${path}lib/popper.js/umd/popper.min.js`,
    `${path}js/mermaid.min.js`,
    `${path}js/video-module.js`,
    `${content}Blazorise/blazorise.js`,
    `${content}Blazorise.Bootstrap/blazorise.bootstrap.js`,
    `${path}_framework/blazor.webassembly.js`
];

const head = document.getElementsByTagName("head")[0];

for (const style of styles) {
    loadStyle(style, () => console.log(`loaded: ${style}`));
}

for (const src of scripts) {
    loadScript(head, src);
}

function loadScript(parent, src) {
    const script = document.createElement("script");
    script.src = src;
    parent.appendChild(script);
}


function loadStyle(href, callback) {
    for (let i = 0; i < document.styleSheets.length; i++) {
        if (document.styleSheets[i].href === href) {
            return;
        }
    }
    const link = document.createElement("link");
    link.rel = "stylesheet";
    link.type = "text/css";
    link.href = href;
    if (callback) { link.onload = function () { callback() } }

    head.insertBefore(link, head.childNodes[0]);

}

let base = document.getElementsByTagName("base")[0];
if (!!!base) {
    base = document.createElement("base");
    document.getElementsByTagName("head")[0].appendChild(base);
}
base.href = `${path}`;