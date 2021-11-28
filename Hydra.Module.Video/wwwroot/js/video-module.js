function HydraTriggerElementClick(cssSelector) {

    const element = document.querySelector(cssSelector);
    if (element) {
        element.click();
    }

}