window.blazorScrollTo = (id) => {
    const el = document.getElementById(id);
    if (!el) return;
    el.scrollIntoView({
        behavior: "smooth",
        block: "start"
    });
};

window.scrollTop = () => {
    window.scrollTo({
        top: 0,
        left: 0,
        behavior: "instant"
    });
}

window.startScrollSpy = (dotnetHelper, sectionIds) => {
    const options = {
        root: null,
        rootMargin: "0px 0px -50% 0px",
        threshold: 0
    };

    const observer = new IntersectionObserver((entries) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                const index = sectionIds.indexOf(entry.target.id);
                if (index !== -1) {
                    dotnetHelper.invokeMethodAsync("UpdateActiveTabFromScroll", index);
                }
            }
        });
    }, options);

    sectionIds.forEach(id => {
        const el = document.getElementById(id);
        if (el) observer.observe(el);
    });

    return observer; // ważne – zwracamy referencję
};

window.stopScrollSpy = (observer) => {
    if (observer) observer.disconnect();
};