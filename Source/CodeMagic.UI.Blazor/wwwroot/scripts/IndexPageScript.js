function setContentSize() {
    const frameElementId = 'index-page-frame';
    const contentElementId = 'index-page-content';

    const contentElement = document.getElementById(contentElementId);
    const frameElement = document.getElementById(frameElementId);

    if (!contentElement || !frameElement) {
        return;
    }

    contentElement.style.height = frameElement.offsetHeight + 'px';
    contentElement.style.width = frameElement.offsetWidth + 'px';

    contentElement.style.left = frameElement.offsetLeft + 'px';
    contentElement.style.top = frameElement.offsetTop + 'px';
}
