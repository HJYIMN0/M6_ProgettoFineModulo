


// Ho aggiunto i seguenti script:
//AbstractFader
//CanvasGroupFader
//GenericSingleton
//SaveSystem
//SaveData
//Save_Monobehaviour
//TimeManager
//Player Input
//PlayerJumpController
//PlayerParticleSystem
//AsbtractEnemy
//SO_EnemyData
//SO_BulletData
//MovableMesh
//iMovable
//Altri...


//Ho modificato i seguenti script:
//TimeManager, rendendolo un singleton
//CoinSystem, aggiungendo il CoinManager
//Tolto Lo script dalla camera, sostituendolo con CineMachine
//PlayerController, Separandolo in 3 differenti scirpts
//PlayerAnimatorController, correggendolo e migliorandolo
//Enemy, facendolo derivare da abstractEnemy e trasformandolo in EnemyStationary
//MovablePlatform



//Questa correzione avrei voluto risolverla, ma più andavo avanti più le codipendenze erano numerose
//E mi sono trovato che avrei dovuto perdere troppo tempo per ricorreggere tutto.

//attenzione alla struttura del Player, le cose di logica dovrebbero essere nella root,
//inoltre il GetComponentInChildren pescava l'_animator sbagliato, perché inizia a cercare dal GameObject stesso


