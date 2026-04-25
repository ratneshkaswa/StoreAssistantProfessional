window.sapActivity = (function () {
    let dotNet = null;
    let last = 0;
    let init = false;

    const ping = () => {
        const now = Date.now();
        if (now - last < 5000) return;
        last = now;
        if (dotNet) dotNet.invokeMethodAsync('NotifyActivity');
    };

    return {
        register: function (dotNetRef) {
            dotNet = dotNetRef;
            if (init) return;
            init = true;
            document.addEventListener('mousemove', ping, { passive: true });
            document.addEventListener('keydown', ping, { passive: true });
            document.addEventListener('mousedown', ping, { passive: true });
            document.addEventListener('touchstart', ping, { passive: true });
        }
    };
})();
