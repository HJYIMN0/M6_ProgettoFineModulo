

//DOING:
//PlayerParticlesSystem
//Finire logica trappole e implementare eventualmente nemico non usato

//TO DO:
//Implementare logica buttons UI
//AGGIUNGERE UN GAME MANAGER CHE GESTISCA IL LIVELLO, L'AUDIO E ALTRO
//Finire la logica di winning trigger aggiungendo il raccoglitore di monete
//Completare il sistema di salvataggio con il vector 3


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


//Ho modificato i seguenti script:
//TimeManager, rendendolo un singleton
//CoinSystem, aggiungendo il CoinManager
//Tolto Lo script dalla camera, sostituendolo con CineMachine
//PlayerController, Separandolo in 3 differenti scirpts
//PlayerAnimatorController, correggendolo e migliorandolo
//Enemy, facendolo derivare da abstractEnemy e trasformandolo in EnemyStationary
//MovablePlatform


//Critiche e miglioramenti proposti dai profs:

//il GameOver canvas non risponde alla pressione dei tasti perché viene coperto dal WinningScenario (anche se trasparente)

//siccome hai messo [SerializeField] protected Bullet _bulletPreFab { get; private set; }
//hai trasformato questa variabile in una property ed è per questo che Unity non la serializza
//e quindi non hai potuto assegnare il prefab da spawnare

//attenzione a non normalizzare l'input del _player se ha magnitudo < 1 perché altrimenti parte subito veloce e
//quando si ferma si ferma in ritardo (senza rallentare)

//non dovresti tenere i canvas sempre tutti attivi

//inoltre non è una buona pratica mettere Time.timeScale = 0 dentro un update come nel caso del
//GameOver_UI perché diventa molto difficile poi andare a controllare il flusso, ad esempio il gioco a volte
//mi si blocca probabilmente per un timeScale = 0 che parte da qualche parte

//le animazioni non avevano l'avatar settato correttamente (solo il modello principale lo aveva,
//gl ialtri dovevano fare copy from other avatar)

//attenzione alla struttura del Player, le cose di logica dovrebbero essere nella root,
//inoltre il GetComponentInChildren pescava l'_animator sbagliato, perché inizia a cercare dal GameObject stesso

//nell' OnCollisionExit() del MovingPlatform _playerRb.velocity = _playerRb.velocity; non fa nulla

//il livello non è completabile (il salto prima del cartellone che dice di correre non si può fare)

//_anim.SetBool(""IsJumping"", true); era messo a true anche nel branch in cui doveva essere false,
//in questi casi è meglio scrivere con una riga sola _anim.SetBool(""IsJumping"", _player._isRunning);

//inoltre, attenzione, non mettere variabili public o property con get public che inizino con _
//quello serve a indicare una variabile visibile solo dentro la classe, al massimo dentro le sottoclassi!

//infine per gestire le animazioni sarebbe meglio fare una cosa più simile all'esempio visto a lezione:
//un blend per la fase di ground tra idle/walk/run, un blend per la fase aerea (ascending/mid/descending)
//un'animazione per il salto (tramite trigger) e una di landing (tramite booleana) per uscire dalla fase aerea"

