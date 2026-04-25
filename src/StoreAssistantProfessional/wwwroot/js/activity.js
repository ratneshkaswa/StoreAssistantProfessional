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

    const onKey = (e) => {
        if (!dotNet) return;
        // Ignore key events while typing in inputs / textareas
        const tag = (e.target && e.target.tagName) || '';
        const isInput = tag === 'INPUT' || tag === 'TEXTAREA' || (e.target && e.target.isContentEditable);

        // Ctrl+number / Ctrl+, / Ctrl+K — fire even from inputs
        if (e.ctrlKey && !e.shiftKey && !e.altKey) {
            const map = { '1': 'dashboard', '2': 'billing', '3': 'products', '4': 'customers', '5': 'reports', ',': 'settings', 'k': 'palette' };
            const k = e.key.toLowerCase();
            if (map[k]) {
                e.preventDefault();
                dotNet.invokeMethodAsync('OnGlobalShortcut', map[k]);
                return;
            }
        }
        // ? overlay — Shift+/
        if (!isInput && e.key === '?') {
            e.preventDefault();
            dotNet.invokeMethodAsync('OnGlobalShortcut', 'help');
            return;
        }
        // Esc — close overlays
        if (e.key === 'Escape') {
            dotNet.invokeMethodAsync('OnGlobalShortcut', 'escape');
        }
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
            document.addEventListener('keydown', onKey);
        }
    };
})();
