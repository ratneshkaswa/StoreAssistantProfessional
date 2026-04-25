window.sapPrint = (html, width, height) => {
    const w = window.open('', '_blank', `width=${width || 420},height=${height || 640}`);
    if (!w) return false;
    w.document.open();
    w.document.write(html);
    w.document.close();
    w.focus();
    w.print();
    return true;
};
