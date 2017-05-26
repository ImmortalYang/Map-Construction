var gulp = require("gulp");
var watch = require("gulp-watch");
var browserSync = require("browser-sync").create();

gulp.task("watch", function () {
    browserSync.init({
        notify: false,
        proxy: "http://localhost:57387",
        files: ["./**/*.cshtml", "./**/*.cs"],
        watchEvents: ["add", "change", "addDir"]
    });

    watch("./wwwroot/css/**/*.less", function () {
        gulp.start("cssInject");
    });

    watch("./wwwroot/js/jsx/**/*.jsx", function () {
        gulp.start("scriptsRefresh");
    });
});

gulp.task('cssInject', ['less'], function () {
    return gulp.src('./wwwroot/css/site.css')
		.pipe(browserSync.stream());
});

gulp.task('scriptsRefresh', ['scripts'], function () {
    browserSync.reload();
});