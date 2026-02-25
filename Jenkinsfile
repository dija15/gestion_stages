pipeline {
    agent any

    options {
        skipDefaultCheckout(false)
    }

    stages {
        stage('Checkout') {
            steps {
                // Nettoyer le workspace avant le checkout
                deleteDir()
                checkout scm
            }
        }

        stage('Debug - Voir fichiers') {
            steps {
                bat 'dir /s'
                bat 'echo Current directory: %CD%'
            }
        }

        stage('Restore') {
            steps {
                // Se placer dans le dossier gestion_stages pour restaurer les projets
                dir('gestion_stages') {
                    bat 'dotnet restore gestion_stages.sln'
                }
            }
        }

        stage('Build') {
            steps {
                dir('gestion_stages') {
                    bat 'dotnet build gestion_stages.sln --configuration Release'
                }
            }
            post {
                failure {
                    echo 'Build failed!'
                }
            }
        }

        stage('Test') {
            steps {
                dir('gestion_stages') {
                    bat 'dotnet test gestion_stages.sln --configuration Release --no-build'
                }
            }
            post {
                failure {
                    echo 'Tests failed!'
                }
            }
        }
    }

    post {
        failure {
            echo 'Pipeline failed!'
        }
        success {
            echo 'Pipeline succeeded!'
        }
    }
}
