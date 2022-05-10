function setContentSize() {
    const frameElementId = "index-page-frame";
    const contentElementId = "index-page-content";
    const widthCheckElementId = "size-check-element";

    const contentElement = document.getElementById(contentElementId);
    const frameElement = document.getElementById(frameElementId);
    const widthCheckElement = document.getElementById(widthCheckElementId);

    if (!contentElement || !frameElement || !widthCheckElement) {
        return;
    }

    const lineHeight = widthCheckElement.offsetHeight;
    const lineWidth = widthCheckElement.offsetWidth;
    const contentHeight = frameElement.offsetHeight - lineHeight * 2;
    const contentWidth = frameElement.offsetWidth - lineWidth * 2;

    contentElement.style.height = contentHeight + "px";
    contentElement.style.width = contentWidth + "px";

    contentElement.style.left = frameElement.offsetLeft + "px";
    contentElement.style.top = frameElement.offsetTop + "px";

    
    contentElement.style.marginTop = lineHeight + "px";
    contentElement.style.marginBottom = lineHeight + "px";
    contentElement.style.marginLeft = lineWidth + "px";
    contentElement.style.marginRight = lineWidth + "px";
}
