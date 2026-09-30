/*!
 * MarqueeVertical v1.0.0
 * Seamless vertical marquee (bottom -> top), no-jank
 * MIT License
 */
(function (global, factory) {
    if (typeof module === "object" && typeof module.exports === "object") {
        module.exports = factory();
    } else {
        global.MarqueeVertical = factory();
    }
})(typeof window !== "undefined" ? window : this, function () {
    class MarqueeVertical {
        /**
         * @param {HTMLElement|string} root - element hoặc CSS selector của container
         * @param {Object} opts
         * @param {number} [opts.speed=40] - tốc độ px/giây (>= 1)
         * @param {boolean} [opts.pauseOnHover=true]
         * @param {number} [opts.gap=0] - khoảng cách giữa 2 bản sao nội dung (px)
         * @param {boolean} [opts.autoStart=true]
         */
        constructor(root, opts = {}) {
            this.root =
                typeof root === "string" ? document.querySelector(root) : root;
            if (!this.root) throw new Error("MarqueeVertical: root not found.");

            this.opts = Object.assign(
                { speed: 40, pauseOnHover: true, gap: 0, autoStart: true },
                opts
            );

            // DOM
            this.track = this.root.querySelector(".marquee-track");
            this.content = this.root.querySelector(".marquee-content");
            if (!this.track || !this.content) {
                // Tự tạo nếu dev chỉ đặt .marquee-vertical và children
                const original = document.createElement("div");
                original.className = "marquee-content";
                while (this.root.firstChild) {
                    original.appendChild(this.root.firstChild);
                }
                const track = document.createElement("div");
                track.className = "marquee-track";
                track.appendChild(original);
                this.root.appendChild(track);
                this.track = track;
                this.content = original;
            }

            // state
            this._raf = null;
            this._lastTs = 0;
            this._offset = 0;
            this._contentHeight = 0;
            this._started = false;
            this._resizeTimer = null;

            // clone để lặp liên tục
            this.clone = this.content.cloneNode(true);
            this.clone.setAttribute("aria-hidden", "true");
            this._applyGap(this.clone, this.opts.gap);
            this.track.appendChild(this.clone);

            // xử lý pause on hover
            if (this.opts.pauseOnHover) {
                this.root.addEventListener("mouseenter", () => (this._hover = true));
                this.root.addEventListener("mouseleave", () => (this._hover = false));
            }

            // hình ảnh cần chờ load để đúng chiều cao
            this._waitImages().then(() => {
                this._measure();
                if (this.opts.autoStart) this.start();
            });

            // responsive
            window.addEventListener("resize", () => {
                clearTimeout(this._resizeTimer);
                this._resizeTimer = setTimeout(() => {
                    const running = this._started;
                    this.stop();
                    this._measure();
                    if (running) this.start();
                }, 120);
            });
        }

        _applyGap(el, gap) {
            if (!gap) return;
            const spacer = document.createElement("div");
            spacer.style.height = gap + "px";
            spacer.style.width = "100%";
            el.appendChild(spacer);
        }

        _waitImages() {
            const imgs = this.root.querySelectorAll("img");
            if (!imgs.length) return Promise.resolve();
            const waiters = Array.from(imgs).map((img) => {
                if (img.complete) return Promise.resolve();
                return new Promise((res) => {
                    img.addEventListener("load", res, { once: true });
                    img.addEventListener("error", res, { once: true });
                });
            });
            return Promise.all(waiters);
        }

        _measure() {
            // đảm bảo track đủ cao chứa 2 bản
            this._contentHeight = this.content.scrollHeight + this.opts.gap;
            this.track.style.height = Math.max(this._contentHeight * 2, this.root.clientHeight) + "px";
            this._offset = 0;
            this._translate(0);
        }

        _translate(y) {
            this.track.style.transform = `translateY(${-y}px)`;
        }

        _tick = (ts) => {
            if (!this._started) return;
            if (!this._lastTs) this._lastTs = ts;
            const dt = Math.max(0, ts - this._lastTs) / 1000; // s
            this._lastTs = ts;

            if (!this._hover) {
                const advance = Math.max(1, this.opts.speed) * dt; // px
                this._offset += advance;
                // khi đi quá 1 bản nội dung, cuộn modulo để seamless
                if (this._offset >= this._contentHeight) {
                    // dùng toán học để tránh nhảy khung hình
                    this._offset = this._offset % this._contentHeight;
                }
                this._translate(this._offset);
            }

            this._raf = requestAnimationFrame(this._tick);
        };

        start() {
            if (this._started) return;
            this._started = true;
            this._lastTs = 0;
            this._raf = requestAnimationFrame(this._tick);
        }

        stop() {
            this._started = false;
            if (this._raf) cancelAnimationFrame(this._raf);
            this._raf = null;
            this._lastTs = 0;
        }

        setSpeed(pxPerSec) {
            this.opts.speed = Number(pxPerSec) || this.opts.speed;
        }
    }

    return MarqueeVertical;
});
