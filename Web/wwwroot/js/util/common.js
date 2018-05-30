define(['jquery', 'bootstrap'], function ($) {
    var _ = {
        lang: $('html').attr('lang') || 'zh',
        /**
         * @param {string} type
         * @param {string} url
         * @param {object} params
         * @param {Function} [callback]
         * @param {Function} [error]
         */
        ajax: function (type, url, params, callback, error) {
            $.ajax({
                type: type,
                url: url,
                data: params,
                async: false,
                dataType: 'json',
                success: function (data, textStatus, jqXHR) {
                    var msg = data['msg'];
                    if (data && data['code'] == 200) {
                        msg && _.alert.success({
                            content: msg,
                            timeout: 3000
                        });
                        $.isFunction(callback) && callback(data['data'] || {}, textStatus, jqXHR);
                    } else {
                        if (data && data['code'] == 302) {
                            window.location.href = '/Home/Login';
                        }
                        if (data && data['code'] == 203) {
                            $.isFunction(callback) && callback(data || {}, textStatus, jqXHR);
                        }
                        else {
                            msg && _.alert.error({
                                id: 'req-error',
                                content: msg,
                                timeout: 3000
                            });
                            $.isFunction(error) && error(data, textStatus, jqXHR);
                        }
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    _.alert.error({
                        content: textStatus
                    });
                    $.isFunction(error) && error(XMLHttpRequest, textStatus, errorThrown);
                }
            });
        },
        /**
         * @param {string} url
         * @param {object} params
         * @param {Function} [callback]
         * @param {Function} [error]
         */
        post: function (url, params, callback, error) {
            _.ajax('post', url, params || {}, callback, error);
        },
        /**
         * @param {string} url
         * @param {object} [params]
         * @param {Function} [callback]
         * @param {Function} [error]
         */
        get: function (url, params, callback, error) {
            _.ajax('get', url, params || {}, callback, error);
        },
        /**
         * Extend bootstrap alert.
         */
        alert: {
            ICONS: {
                info: 'fa-info-circle',
                success: 'fa-check-circle',
                warning: 'fa-exclamation-circle',
                danger: 'fa-times-circle'
            },
            /**
             * Pop out alert components as bootstrap defined.
             * @param {object} settings The settings as defaults defined.
             * @param {string} type The type should be info, success, warning and error.
             * @private
             */
            _alert: function (settings, type) {
                var defaults = {
                    id: '',
                    container: 'body',
                    title: '',
                    content: '',
                    closeable: true,
                    timeout: -1,
                    onShown: function () {
                    },
                    onClose: function (e) {
                    },
                    onClosed: function (e) {
                    }
                };

                (typeof settings === 'string') || (typeof settings === 'number') ? defaults.content = settings : (defaults = $.extend(true, defaults, settings));

                var html = [],
                    id = defaults.id || 'alert-' + Date.now();
                if ($('#' + id).length) {
                    $('#' + id).remove();
                    // return;
                }

                html.push('<div id="' + id + '" class="alert alert-' + type + ' alert-dismissible" role="alert">');
                defaults.closeable && html.push('<button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">×</span></button>');
                defaults.title && html.push('<h4>' + defaults.title + '</h4>');
                html.push('<span><i class="fa ' + _.alert.ICONS[type] + '"></i> ' + defaults.content + '</span>');
                html.push('</div>');

                $(defaults.container).append(html.join(''));
                var alert = $('#' + id);
                'body' === defaults.container && alert.hide().slideDown('fast');

                defaults.container === 'body' && (alert.addClass('alert-ext').css({'margin-left': ((alert.width() + 40) / -2) - 47}));

                $.isFunction(defaults.onShown) && defaults.onShown();

                alert.on('close.bs.alert', function (e) {
                    $.isFunction(defaults.onClose) && defaults.onClose(e);
                });

                alert.on('closed.bs.alert', function (e) {
                    $.isFunction(defaults.onClosed) && defaults.onClosed(e);
                });

                defaults.timeout > 0 && (setTimeout(function () {
                    alert.alert('close');
                }, defaults.timeout));
            },
            /**
             * Pop out a blue alert.
             * @param {object} settings
             */
            info: function (settings) {
                _.alert._alert(settings, 'info');
            },
            /**
             * Pop out a green alert.
             * @param {object} settings
             */
            success: function (settings) {
                _.alert._alert(settings, 'success');
            },
            /**
             * Pop out a yellow alert.
             * @param {object} settings
             */
            warning: function (settings) {
                _.alert._alert(settings, 'warning');
            },
            /**
             * Pop out a red alert.
             * @param {object} settings
             */
            error: function (settings) {
                _.alert._alert(settings, 'danger');
            }
        },
        /**
         * Extend bootstrap dialog.
         */
        dialog: {
            /**
             * Confirm modal dialog
             * @param {object} options {backdrop, keyboard, show, onShow, onShown, onHide, onHidden, onConfirm}
             */
            confirm: function (options) {
                var defaults = {
                        backdrop: true,
                        keyboard: true,
                        show: true,
                        ariaHidden: true,
                        modalClass: 'modal-md',
                        onShow: function (e) {
                        },
                        onShown: function (e) {
                        },
                        onHide: function (e) {
                        },
                        onHidden: function (e) {
                        },
                        onConfirm: function (e) {
                        }
                    },
                    settings = $.extend(true, defaults, options || {});

                var html = '<div id="dialog-confirm" class="modal fade"  tabindex="-1" role="dialog"><div class="modal-dialog"><div class="modal-content"><div class="modal-header"><button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button><h4 class="modal-title">' + (settings['title'] || '') + '</h4></div><div class="modal-body">' + (settings['content'] || '') + '</div><div class="modal-footer">';

                settings['confirm'] && (html += '<button id="dialog-confirm-btn" type="button" class="btn btn-primary">' + (settings['confirm'] || '') + '</button> ');
                settings['cancel'] && (html += ' <button type="button" class="btn btn-default" data-dismiss="modal">' + (settings['cancel'] || '') + '</button>');

                html += '</div></div></div></div><div id="dialog-confirm-backdrop" class="modal-backdrop fade in"></div>';

                $('body').append(html);
                var modal = $('#dialog-confirm');

                modal.find('.modal-dialog').addClass(settings.modalClass);
                modal.modal(settings);

                modal.on('show.bs.modal', function (e) {
                    $.isFunction(settings.onShow) && settings.onShow(e);
                });

                modal.on('shown.bs.modal', function (e) {
                    $.isFunction(settings.onShown) && settings.onShown(e);
                });

                modal.on('hide.bs.modal', function (e) {
                    $.isFunction(settings.onHide) && settings.onHide(e);
                });

                modal.on('hidden.bs.modal', function (e) {
                    $.isFunction(settings.onHidden) && settings.onHidden(e);
                    modal.remove();
                    $('#dialog-confirm-backdrop').remove();
                });

                modal.find('#dialog-confirm-btn').click(function (e) {
                    $.isFunction(settings.onConfirm) && settings.onConfirm(e);
                    modal.modal('hide');
                });
            }
        },
        /**
         * Extend datatables functionality.
         */
        table: {
            /**
             * Override an object from array which key-val equals conditionals.
             * @param {Array} target
             * @param {string} key
             * @param {string|number} val
             * @param {object} obj
             */
            overrideObject: function (target, key, val, obj) {
                for (var i = 0, size = target.length; i < size; i++) {
                    var o = target[i];
                    if (o[key] === val) {
                        target[i] = $.extend(true, o, obj);
                        break;
                    }
                }
            },
            /**
             * Create checkbox for datatables
             * @param {string} tableId
             * @param {Array} columns
             * @returns {{lang: (*|string), ajax: ajax, post: post, get: get, table: {overrideObject: overrideObject, renderCheckboxColumn: renderCheckboxColumn, getCheckedIds: getCheckedIds, dataTablesLangFile: dataTablesLangFile, createButton: createButton}, formatDate: formatDate}}
             */
            renderCheckboxColumn: function (tableId, columns) {
                columns.unshift({
                    data: '_Id',
                    width: '20px',
                    title: '<div class="text-center"><input type="checkbox" id="' + tableId + '-toggle-chk"></div>',
                    sortable: false,
                    render: function (data, type, full, meta) {
                        return '<div class="text-center"><input type="checkbox" class="j-toggle-chk" data-id="' + full['Id'] + '"></div>';
                    }
                });

                var table = $('#' + tableId).delegate('#' + tableId + '-toggle-chk', 'click', function () {
                    table.find('.j-toggle-chk').prop('checked', $(this).is(':checked'));
                });

                return _;
            },
            renderRadioColumn: function (tableId, columns) {
                columns.unshift({
                    data: '_Id',
                    width: '20px',
                    //title: '<div class="text-center"><input type="radio" name="'+tableId+'" id="' + tableId + '-toggle-chk"></div>',
                    sortable: false,
                    render: function (data, type, full, meta) {
                        return '<div class="text-center"><input type="radio" name="' + tableId + '"  class="j-toggle-chk" data-id="' + full['Id'] + '"></div>';
                    }
                });

                var table = $('#' + tableId).delegate('#' + tableId + '-toggle-chk', 'click', function () {
                    table.find('.j-toggle-chk').prop('checked', $(this).is(':checked'));
                });

                return _;
            },
            /**
             * Get checkboxes Id array from the table
             * @param {string} tableId
             * @returns {Array}
             */
            getCheckedIds: function (tableId) {
                var Ids = [];
                $('#' + tableId).find('.j-toggle-chk').each(function () {
                    var self = $(this);
                    self.is(':checked') && Ids.push(self.data('id'));
                });

                return Ids;
            },
            /**
             * Get datatables language file
             * @returns {string}
             */
            dataTablesLangFile: function () {
                return '/assets/plugin/datatables-plugins/i18n/' + _.lang + '.json';
            }
        },
        /**
         * Check whether the rang contains the key.
         * @param {Array} rang
         * @param {number|string} key
         * @returns {boolean}
         */
        indexOf: function (rang, key) {
            if (typeof rang['indexOf'] === 'function') {
                return rang.indexOf(key) != -1;
            }
            for (var i = 0, size = rang.length; i < size; i++) {
                if (rang[i] === key) {
                    return true;
                }
            }
            return false;
        },
        /**
         * Format date object to string
         *
         * @param {Date|number} dt
         * @param {String} [fmt] e.g.: 'yyyy-MM-dd hh:mm:ss'
         * @return {String}
         */
        formatDate: function (dt, fmt) {
            if (!dt) {
                return '';
            }
            typeof dt === 'number' && (dt = new Date(dt));
            !fmt && (fmt = 'yyyy-MM-dd hh:mm:ss');

            var z = {
                M: dt.getMonth() + 1,
                d: dt.getDate(),
                h: dt.getHours(),
                m: dt.getMinutes(),
                s: dt.getSeconds()
            };
            fmt = fmt.replace(/(M+|d+|h+|m+|s+)/g, function (v) {
                return ((v.length > 1 ? "0" : "") + eval('z.' + v.slice(-1)))
                .slice(-2);
            });
            return fmt.replace(/(y+)/g, function (v) {
                return dt.getFullYear().toString().slice(-v.length);
            });
        },
        /**
         * Format date string as  "/Date(1459999067000)/"
         * @param {string|Date} datetime
         * @param {string} [fmt]
         * @returns {String}
         */
        formatRegDate: function (datetime, fmt) {
            if (!datetime) {
                return datetime;
            }

            if (datetime instanceof Date) {
                return _.formatDate(datetime, fmt);
            }

            return datetime.indexOf('Date') != -1 ? _.formatDate(Number((datetime || '').replace(/[^\d]/g, '')), fmt) : _.formatDate(new Date(datetime), fmt);
            //return _.formatDate(Number((str || '').replace(/[^\d]/g, '')), fmt);
        },
        /**
         * Format viewer counter.
         * @param {Number} number - Viewer counter.
         * @returns {string}
         */
        formatSymbolInt: function (number) {
            return number && number.toString().replace(/(?=(?!^)(\d{3})+$)/g, ',');
        },
        formatSymbol: function (number) {
            //return number && number.toString().replace(/(?=(?!^)(\d{3})+$)/g, ',');
            if (number === "") return "";
            var result = '', strnum = parseFloat(number).toFixed(2).toString();
            if (number < 0) {
                result += '-';
                strnum = strnum.substring(1);
            }

            var arr = strnum.split('.');
            result += arr[0].replace(/(?=(?!^)(\d{3})+$)/g, ',');
            result += '.' + arr[1];
            return result;
        },
        /**
         * Get url param value by name
         * @param {string} name
         * @returns {string}
         */
        getParameter: function (name) {
            name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
            var regexS = "[\\?&]" + name + "=([^&#]*)",
                regex = new RegExp(regexS),
                results = regex.exec(window.location.search);
            return (results == null ? '' : decodeURIComponent(results[1].replace(/\+/g, " ")));
        },
        /**
         * Set url param.
         * @param {string} key
         * @param {string} val
         * @param {string} [url]
         * @returns {string}
         */
        setParameter: function (key, val, url) {
            var href = url || window.location.href,
                index = href.indexOf('?'),
                param = key + '=' + encodeURIComponent(val);
            return index > -1 ? href + '&' + param : href + ('?' + param);
        },
        /**
         * Template format tools.
         * usage:
         * 1).template('I can find {0}!, your id is: {1}-{1}-{2}-{3}', 'Chiroc', 22322, 4353, 'IIUTYG')
         *  output: I can find Chiroc!, your id is: 22322-22322-4353-IIUTYG
         * 2).template('I can find your {name}, {age}, {title.main}, {title.mains}, {title.sub-title.tip}', {name: 'Chiroc', age: 24, title: {main: 0, 'sub-title': {tip: 'YUI'}}})
         *  output: I can find your Chiroc, 24, 0, {title.mains}, YUI
         * @param {string} text The value's indicators, can be HTML or plain text with {0}, {1}, {name}, or {object.name} format.
         * @param {string | object} settings Indicators value.
         * @returns {XML|string|void}
         */
        template: function (text, settings) {
            var interpolate = /\{(.+?)\}/g,
                isObj = typeof settings === 'object',
                format = function () {
                    var s = '', size = arguments[0].length, i = 1;
                    s = arguments[0][0];
                    for (; i < size; i++) {
                        s = s.split('{' + (i - 1) + '}').join(arguments[0][i]);
                    }

                    return s;
                },
                getValue = function (namespace, match, settings) {
                    var keys = namespace.split('.'),
                        _getValue = function (keys, settings) {
                            for (var i = 0, size = keys.length; i < size; i++) {
                                var key = keys[i];
                                if (typeof settings[key] !== 'object') {
                                    return settings[key];
                                } else {
                                    settings = settings[key];
                                }
                            }
                            return match;
                        }, result = _getValue(keys, settings);

                    return (result === undefined ? match : result);
                };

            if (arguments.length < 2) {
                return text;
            }

            if (!isObj) {
                return format(arguments);
            }

            return arguments.length == 2 && isObj && text.replace(interpolate, function (match, escape, interpolate, evaluate, offset) {
                    return escape.split('.').length < 2 ? (isObj ? (settings[escape] == undefined ? '' : settings[escape]) : settings) : getValue(escape, match, settings);
                });
        },
        /**
         * Get/Set cookie
         * @param {string} key
         * @param {string|number} value
         * @param {string} [path]
         * @param {number} [expire] Days of expire.
         * @returns {string|object|undefined}
         */
        cookie: function (key, value, path, expire) {
            var get = function (key) {
                return decodeURIComponent(document.cookie.replace(new RegExp('(?:(?:^|.*;\\s*)' + encodeURIComponent(key) + '\\s*\\=\\s*([^;]*).*$)|^.*$'), '$1')) || '';
            }, set = function (key, value, path, expire) {
                var d = new Date();
                d.setDate(d.getDate() + expire);
                var c = encodeURIComponent(key) + '=' + encodeURIComponent(value) + (expire ? ';expires=' + d.toGMTString() : '');
                document.cookie = c + (path ? ('; path=' + path) : '');//';path=/'
            };

            if (arguments.length == 1) {
                return get(key);
            } else if (arguments.length > 1) {
                set(key, value, path, expire);
                return _;
            }
        },
        /**
         * Remove cookie.
         * @param {string} key
         */
        removeCookie: function (key) {
            document.cookie = encodeURIComponent(key) + '=;expires=' + new Date(0).toGMTString() + ';';
            return _;
        },
        /**
         * Filter illegal characters. Set input class as 'v-illegal' to enable illegal chars validating.
         * @param {jQuery} [objects]
         */
        filterIllegalChars: function (objects) {
            (objects || $('.v-illegal')).each(function () {
                var self = $(this),
                    replace = function () {
                        var val = self.val();
                        val.match(/[`~!^|<>,;'"]/ig) && self.val(val.replace(/[`~!^|<>,;'"]/ig, ''));
                    };
                self.on('blur', function (e) {
                    ///[`~!^|<>;'"]/.test(e.key) && e.preventDefault();
                    replace();
                });
                self.on('keyup', function (e) {
                    //e.keyCode == 13 && replace();
                    replace();
                });
                //.on('input', function (e) {
                //    self.val(self.val().replace(/[`~!^|<>;'"]/ig, ''));
                //});
            });
        },
        _ES_PLUGINS: function () {
            if (!Object.keys) Object.keys = function (o) {
                if (o !== Object(o))
                    throw new TypeError('Object.keys called on non-object');
                var ret = [], p;
                for (p in o) if (Object.prototype.hasOwnProperty.call(o, p)) ret.push(p);
                return ret;
            }
        },
        /**
         * Checking mobile device.
         * code from https://github.com/google/vrview/blob/master/src/util.js
         * @returns {boolean}
         */
        isMobile: function () {
            var check = false;
            (function (a) {
                if (/(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino/i.test(a) || /1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-/i.test(a.substr(0, 4))) check = true
            })(navigator.userAgent || navigator.vendor || window.opera);
            return check;
            //return /Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent);
        },
        validateNumber: function (objects) {
            (objects || $('.v-number')).each(function () {
                var self = $(this), replace = function () {
                    self.val(self.val().replace(/\D/g, ''));

                    var number = parseInt(self.val(), 10);
                    if (number < 1) {
                        self.val(1);
                        return;
                    }
                };
                self.on('input', function (e) {
                    replace();
                });
                self.on('blur', function (e) {
                    replace();
                });
                self.on('keypress', function (e) {
                    e.keyCode == 13 && replace();
                });
            });
        },
        /**
         * {seconds, target, format=['ss','mm:ss', 'hh:mm:ss'], onStart, onEnd}
         * @param {Object} settings
         */
        ticking: function (settings) {
            var defaults = {
                    target: '',
                    seconds: 10,
                    format: 'ss',
                    message: '{0}',
                    onStart: function (e) {
                    },
                    onEnd: function (e) {
                    }
                },
                completeChar = function (val) {
                    return new String('00').substring((val + '').length, 2) + val;
                }, handler = {
                    'ss': function (s) {
                        return s;
                    },
                    'mm:ss': function (s) {
                        return completeChar(Math.floor(s / 60)) + ':' + completeChar(s % 60);
                    },
                    'hh:mm:ss': function (s) {
                        return completeChar(Math.floor(s / 3600)) + ':' + completeChar(Math.floor(s % 3600 / 60)) + ':' + completeChar(s % 60);
                    }
                },

                nodeNames = {
                    'INPUT': 'val',
                    'BUTTON': 'html',
                    'TEXTAREA': 'val'
                };

            defaults = $.extend(true, defaults, settings);
            if (!defaults.seconds) {
                return;
            }
            var nodeName = defaults.target[0].nodeName;

            $.isFunction(defaults.onStart) && defaults.onStart(defaults.target);

            defaults.seconds++;
            var tick = setInterval(function () {
                defaults.seconds -= 1;
                defaults.target[nodeNames[nodeName] || 'html'](_.template(defaults.message, handler[defaults.format](defaults.seconds)));
                !defaults.seconds && (clearInterval(tick), $.isFunction(defaults.onEnd) && defaults.onEnd(defaults.target));
            }, 1000);
        }
    };


    $(function () {
        _.filterIllegalChars();
        _._ES_PLUGINS();
        _.validateNumber();
    });

    /**
     * Export APIs
     */
    return {
        lang: _.lang,
        post: _.post,
        get: _.get,
        indexOf: _.indexOf,
        dialog: {
            confirm: _.dialog.confirm
        },
        alert: {
            info: _.alert.info,
            warning: _.alert.warning,
            success: _.alert.success,
            error: _.alert.error
        },
        table: {
            overrideObject: _.table.overrideObject,
            renderCheckboxColumn: _.table.renderCheckboxColumn,
            dataTablesLangFile: _.table.dataTablesLangFile,
            getCheckedIds: _.table.getCheckedIds,
            renderRadioColumn: _.table.renderRadioColumn
        },
        template: _.template,
        getParameter: _.getParameter,
        setParameter: _.setParameter,
        formatSymbolInt: _.formatSymbolInt,
        formatSymbol: _.formatSymbol,
        cookie: _.cookie,
        isMobile: _.isMobile,
        removeCookie: _.removeCookie,
        formatDate: _.formatDate,
        formatRegDate: _.formatRegDate,
        filterIllegalChars: _.filterIllegalChars,
        ticking: _.ticking
    };
});