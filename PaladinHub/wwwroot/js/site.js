/* ========== CART BADGE (red bubble) ========== */
window.updateCartBadge = window.updateCartBadge || function (count, animate) {
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
};

window.fetchCartCount = window.fetchCartCount || async function () {
    try {
        const res = await fetch('/Cart/CountJson', { credentials: 'same-origin' });
        if (!res.ok) throw new Error();
        const count = await res.json();
        window.updateCartBadge(count, false);
    } catch {
        window.updateCartBadge(0, false);
    }
};

/* ========== MINI CART (hover panel) ========== */
let miniLoaded = false, hoverTimer = null;

async function refreshMiniCart() {
    const panel = document.getElementById('mini-cart-panel');
    if (!panel) return;
    try {
        const res = await fetch('/Cart/Mini', { credentials: 'same-origin' });
        panel.innerHTML = await res.text();
    } catch (e) {
        console.error('Mini cart refresh failed', e);
    }
}

async function showMiniCart() {
    const panel = document.getElementById('mini-cart-panel');
    if (!panel) return;
    panel.classList.remove('d-none');
    if (!miniLoaded) {
        await refreshMiniCart();
        miniLoaded = true;
    }
}
function hideMiniCart() {
    const panel = document.getElementById('mini-cart-panel');
    if (!panel) return;
    panel.classList.add('d-none');
}

/* ========== MINI CART: remove (X) with delegation ========== */
document.addEventListener('click', async (e) => {
    const btn = e.target.closest('.mini-remove');
    if (!btn) return;
    e.preventDefault();

    const id = btn.dataset.id;
    if (!id) return;

    const token = document.querySelector("input[name='__RequestVerificationToken']")?.value;

    try {
        const resp = await fetch(`/Cart/RemoveProduct?id=${encodeURIComponent(id)}`, {
            method: 'POST',
            credentials: 'same-origin',
            headers: Object.assign(
                { 'X-Requested-With': 'XMLHttpRequest' },
                token ? { 'RequestVerificationToken': token } : {}
            )
        });
        const data = await resp.json();
        if (data?.ok) {
            await refreshMiniCart();
            await window.fetchCartCount?.();
        }
    } catch (err) {
        console.error('Mini remove failed', err);
    }
});

/* ========== SYNC triggered from MyCart.cshtml ========== */
document.addEventListener('cart:changed', async () => {
    try {
        await window.fetchCartCount?.();
        await refreshMiniCart?.();
    } catch (e) { console.error(e); }
});

/* ========== ON LOAD: set up hover, refresh badge, auto-hide alerts ========== */
document.addEventListener('DOMContentLoaded', () => {
    // Hover wiring for the mini cart host
    const host = document.getElementById('nav-cart');
    if (host) {
        host.addEventListener('mouseenter', () => {
            clearTimeout(hoverTimer);
            showMiniCart();
        });
        host.addEventListener('mouseleave', () => {
            hoverTimer = setTimeout(hideMiniCart, 200);
        });
    }

    // Always refresh badge on page load
    window.fetchCartCount && window.fetchCartCount();

    // Refresh badge when user returns to the tab
    document.addEventListener('visibilitychange', () => {
        if (!document.hidden) {
            window.fetchCartCount && window.fetchCartCount();
        }
    });

    // Animate badge if product was just added (server sets this flag)
    if (window.__cartJustAdded__ === true || window.__cartJustAdded__ === "true") {
        window.fetchCartCount().then(() => {
            const n = parseInt(document.getElementById('cart-badge')?.textContent || '0', 10);
            window.updateCartBadge(n, true);
        });
    }

    // Auto-hide floating alerts
    document.querySelectorAll(".alert-custom").forEach(el => {
        setTimeout(() => el.remove(), 3500);
    });
});
