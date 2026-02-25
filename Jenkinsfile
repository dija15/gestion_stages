pipeline {
    agent any

    stages {
        stage('Checkout') {
            steps {
                deleteDir()
                checkout scm
            }
        }

        stage('Debug - Voir fichiers') {
            steps {
                bat 'dir /s'
            }
        }

        stage('Restore') {
            steps {
                dir('gestion_stages') {
                     bat 'dotnet restore gestion_stages.sln --no-cache --verbosity detailed'
                 }
           }
        }

        stage('Build') {
            steps {
                script {
                    def projects = findFiles(glob: 'gestion_stages/**/*.csproj')
                    for (p in projects) {
                        echo "Building ${p.path}"
                        bat "dotnet build \"${p.path}\" --configuration Release"
                    }
                }
            }
        }

        stage('Test') {
            steps {
                script {
                    def projects = findFiles(glob: 'gestion_stages/**/*.csproj')
                    for (p in projects) {
                        // On teste seulement si le projet contient des tests
                        if (p.path.toLowerCase().contains("test")) {
                            echo "Testing ${p.path}"
                            bat "dotnet test \"${p.path}\" --no-build --configuration Release"
                        }
                    }
                }
            }
        }
    }

    post {
        failure { echo 'Pipeline failed!' }
        success { echo 'Pipeline succeeded!' }
    }
}
