 (function () {
    // SignalR chat client with lazy DOM lookup so the chat panel can be injected dynamically.
    const connection = new signalR.HubConnectionBuilder().withUrl('/chathub').build();

    function getEl(id) {
        return document.getElementById(id);
    }

    function escapeHtml(text) {
        if (!text) return '';
        return text
            .replace(/&/g, '&amp;')
            .replace(/</g, '&lt;')
            .replace(/>/g, '&gt;')
            .replace(/"/g, '&quot;')
            .replace(/'/g, '&#039;');
    }

    function appendMessage(user, msg, time) {
        const messagesEl = getEl('liveChatMessages');
        if (!messagesEl) return;
        const el = document.createElement('div');
        el.className = 'mb-2';
        el.innerHTML = `<div class="small text-muted">${escapeHtml(user)} <span class='ms-2'>${new Date(time).toLocaleTimeString()}</span></div><div>${escapeHtml(msg)}</div>`;
        messagesEl.appendChild(el);
        messagesEl.scrollTop = messagesEl.scrollHeight;
    }

    connection.on('ReceiveMessage', (user, message, time) => {
        appendMessage(user, message, time);
    });

    connection.on('LoadHistory', (items) => {
        if (!items) return;
        items.forEach(i => {
            // support different shapes from server
            const user = i.User || i.user || 'Guest';
            const message = i.Message || i.message || '';
            const time = i.Time || i.time || new Date().toISOString();
            appendMessage(user, message, time);
        });
    });

    connection.start().catch(err => console.error('SignalR start error:', err));

    // Expose global method that looks up inputs at call-time (works with dynamically inserted UI)
    window.sendLiveChat = function () {
        try {
            if (!connection || connection.state !== signalR.HubConnectionState.Connected) {
                console.warn('Chat not connected yet.');
                return;
            }
            const inputUser = getEl('liveChatUser');
            const inputMessage = getEl('liveChatInput');
            const user = inputUser ? inputUser.value.trim() : 'Guest';
            const msg = inputMessage ? inputMessage.value.trim() : '';
            if (!msg) return;
            connection.invoke('SendMessage', user || 'Guest', msg).catch(err => console.error(err));
            if (inputMessage) inputMessage.value = '';
        } catch (e) {
            console.error(e);
        }
    };
})();
