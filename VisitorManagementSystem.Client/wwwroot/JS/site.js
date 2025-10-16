// wwwroot/js/site.js
window.closePopupAndNavigate = (url) => {
    try {
        // Hide modal manually
        document.querySelectorAll('.modal.show').forEach(m => {
            m.classList.remove('show');
            m.style.display = 'none';
        });

        // Remove the backdrop if one exists
        document.querySelectorAll('.modal-backdrop').forEach(b => b.remove());

        // Wait a moment before navigating
        setTimeout(() => window.location.href = `${window.location.origin}/${url}`, 200);
    } catch (err) {
        console.error("Error closing popup or navigating:", err);
    }
};
