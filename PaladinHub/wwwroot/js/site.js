(function () {
    function updateCartBadge(count, animate) {
        var badge = document.getElementById('cart-badge');
        if (!badge) return;
        if (!count || count <= 0) {
            badge.classList.add('d-none');
            badge.textContent = '0';
            return;
        }
        badge.textContent = count;
        badge.classList.remove('d-none');
        if (animate) {
            badge.classList.add('cart-badge-pop');
            setTimeout(function () { badge.classList.remove('cart-badge-pop'); }, 300);
        }
    }

    async function fetchCartCount() {
        try {
            const res = await fetch('/Cart/CountJson', { credentials: 'same-origin' });
            if (!res.ok) throw new Error();
            const count = await res.json();
            updateCartBadge(count, false);
        } catch {
            updateCartBadge(0, false);
        }
    }

    let miniLoaded = false, hoverTimer = null;

    async function showMiniCart() {
        const panel = document.getElementById('mini-cart-panel');
        if (!panel) return;
        panel.classList.remove('d-none');

        if (!miniLoaded) {
            const res = await fetch('/Cart/Mini', { credentials: 'same-origin' });
            panel.innerHTML = await res.text();
            miniLoaded = true;
        }
    }

    function hideMiniCart() {
        const panel = document.getElementById('mini-cart-panel');
        if (!panel) return;
        panel.classList.add('d-none');
    }

    document.addEventListener('DOMContentLoaded', function () {
        const host = document.getElementById('nav-cart');
        if (!host) return;

        // първоначален брой
        fetchCartCount();

        // анимация след AddProduct
        if (window.__cartJustAdded__ === true || window.__cartJustAdded__ === "true") {
            fetchCartCount().then(() => updateCartBadge(
                parseInt(document.getElementById('cart-badge').textContent || '0', 10), true));
        }

        host.addEventListener('mouseenter', () => {
            clearTimeout(hoverTimer);
            showMiniCart();
        });
        host.addEventListener('mouseleave', () => {
            hoverTimer = setTimeout(hideMiniCart, 200);
        });
    });
})();
