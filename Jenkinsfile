pipeline {
    agent any

    environment {
        DOTNET_CLI_TELEMETRY_OPTOUT = "1"
        DOTNET_SKIP_FIRST_TIME_EXPERIENCE = "1"
    }

    stages {
        stage('Restore') {
            steps {
                echo 'Nettoyage des caches NuGet et restauration des packages...'
                bat 'dotnet nuget locals all --clear'
                bat 'dotnet restore gestion_stages.sln'
            }
        }

        stage('Build') {
            steps {
                echo 'Compilation de la solution...'
                bat 'dotnet build gestion_stages.sln --configuration Debug'
            }
        }

        stage('Test') {
            steps {
                echo 'Exécution des tests...'
                // Si tu as des projets de test, ajuste le chemin
                // Exemple: bat 'dotnet test gestion_stages/tests/UnitTests/UnitTests.csproj --configuration Debug'
                bat 'dotnet test gestion_stages.sln --configuration Debug'
            }
        }

        stage('Debug Info') {
            steps {
                echo 'Liste des fichiers compilés pour vérification...'
                bat 'dir /s gestion_stages\\bin'
                bat 'dir /s gestion_stages\\obj'
            }
        }
    }

    post {
        always {
            echo 'Pipeline terminé.'
        }
        success {
            echo 'Build réussi !'
        }
        failure {
            echo 'Build échoué.'
        }
    }
}
