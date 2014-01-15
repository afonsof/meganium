module.exports = function (grunt) {
    'use strict';

    // load all grunt tasks
    var compiledTsFilePath = 'Site/Content/admin/js/app.js';
    var slnPath = 'Meganium.sln';
    grunt.initConfig({
    
        execOpts: {
          maxBuffer: Infinity
        },

        pkg: grunt.file.readJSON('package.json'),

        clean: {
            compile: ['Site/Temp'],
        },

        ts: {
            options: {
                target: 'es3',
                module: 'amd',
                sourcemap: true,
                declaration: false,
                nolib: false,
                comments: false
            },
            live: {
                src: ['Site/Content/admin/js/src/*.ts'],
                out: compiledTsFilePath,
                options: {
                    sourcemap: false
                },
            },
        },

        uglify: {
            my_target: {
                files: {
                    'Site/Content/admin/js/app.min.js': [compiledTsFilePath]
                }
            }
        },

        assemblyinfo: {
            options: {
                files: [
                    'Site/Site.csproj',
                    'Plugins/Plugins.csproj',
                    'Api/Api.csproj',
                    'Installer/Installer.csproj',
                    'Tests/SystemTests/SystemTests.csproj',
                    'Tests/UnitTests/UnitTests.csproj'
                ],
                info: {
                    version: "<%= pkg.version %>.0",
                    fileVersion: "<%= pkg.version %>.0",
                    company: 'Meganium',
                    product: 'Meganium Smart Site',
                    copyright: 'Copyright 2014 (c) Meganium',
                }
            }
        },

        msbuild: {
            src: [slnPath],
            options: {
                projectConfiguration: 'Release',
                targets: ['Clean', 'Rebuild'],
                stdout: true,
                verbosity: 'minimal',
                buildParameters: {
                    WarningLevel: 3,
                    DeployOnBuild: true,
                    PublishProfile: "temp"
                },
            }
        },

        nunit: {
            options: {
                files: ['./Tests/UnitTests/UnitTests.csproj',
                        './Tests/SystemTests/SystemTests.csproj']
                //teamcity: true
            }
        },

        ftpush: {
            build: {
                auth: {
                    host: 'afonsof.com',
                    port: 21,
                    authKey: 'key1'
                },
                src: 'Site/Temp',
                dest: '/',
                exclusions: ['Site/**/*.cs'],
                keep: ['/Content/Uploads'],
                simple: true,
                useList: true
            }
        }

    });

    loadTasks(grunt);

    grunt.registerTask('ci', ['assemblyinfo', 'msbuild', 'nunit']);
    grunt.registerTask('ts-compile', ['ts:live', 'uglify']);
};

function loadTasks(grunt) {
    grunt.loadNpmTasks('grunt-ts'); //compiles typescript
    grunt.loadNpmTasks('grunt-contrib-uglify'); // minifies
    grunt.loadNpmTasks('grunt-contrib-copy'); // copy files
    grunt.loadNpmTasks('grunt-contrib-clean'); // erase files and folders
    grunt.loadNpmTasks('grunt-contrib-concat'); // concat files
    grunt.loadNpmTasks('grunt-contrib-jshint'); // lint js files
    grunt.loadNpmTasks('grunt-contrib-qunit'); // unit tests support
    grunt.loadNpmTasks('grunt-closure-compiler'); //closure compiler
    grunt.loadNpmTasks('grunt-dotnet-assembly-info'); //change assembly info
    grunt.loadNpmTasks('grunt-msbuild'); //msbuild grunt task
    grunt.loadNpmTasks('grunt-nunit-runner');
    grunt.loadNpmTasks('grunt-ftpush');
}