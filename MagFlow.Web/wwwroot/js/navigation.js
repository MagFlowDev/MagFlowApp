window.blazorScrollTo = (id) => {
    const el = document.getElementById(id);
    if (!el) return;
    el.scrollIntoView({
        behavior: "smooth",
        block: "start"
    });
};

window.startScrollSpy = (dotnetHelper, sectionIds) => {
    const options = {
        root: null,
        rootMargin: "0px 0px -70% 0px",
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