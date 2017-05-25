var gulp = require('gulp');
var less = require('gulp-less');
var autoprefixer = require('autoprefixer');

gulp.task('less', function () {
    return gulp.src('./wwwroot/css/site.less')
        .pipe(less([autoprefixer]))
        .on('error', function (errorInfo) {
            console.log(errorInfo.toString());
            this.emit('end');
        })
        .pipe(gulp.dest('./wwwroot/css'));
});