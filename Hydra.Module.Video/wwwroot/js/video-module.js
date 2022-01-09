var JsFunctions = window.JsFunctions || {};

JsFunctions = {
    MermaidInitialize: function () {
        mermaid.initialize({
            startOnLoad: true,
            securityLevel: "loose"
        });
    },

    MermaidRender: function () {
        mermaid.init();
    }
};