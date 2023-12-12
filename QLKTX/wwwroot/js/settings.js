
//other storage
function OtherStorage() {
    this.get = function (key) {
        return JSON.parse(window.localStorage.getItem(key));
    };

    this.set = function (key, value) {
        window.localStorage.setItem(key, JSON.stringify(value));
    };

    this.clear = function (key) {
        window.localStorage.removeItem(key);
    };
}