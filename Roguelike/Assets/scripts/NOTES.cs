/*
_____________________________________________________________________________________________
SPRITE
    *\M indicates uses sprite mask
  
    lessPrior
        -200: floor tiles
        -150: level imprint on floor
        -90: turret holes
        -85: conc slabs
        -80: 8chest octPlat \M;  terrain
        -1..0: tutorial WASD to move
        50: flame
    Default
        0: most enemy bases;  crates and steel boxes
        2: buzz tank buzzes

        10..15: vase guy
            15: vase guy eyes
        20..21: 8chest chests \M
            21: 8chest instRing
        20: aug item
            22: shop counter display
        25: augOffer \M
        30..32: observatory
        50..53: force field  \M
            51-52: force field hp \m
            53: ff flash
        150: forge
        300: surge lightning bolt
    Player
        -20: robot pull shadow (conc slab pull enter)
        -10: item arrow line ind \M
        10: player robot leg
        20: weapons \M swapAnim masks
        22: swapAnim laser \M
        30: player robot torso
        32: torso T2 glow \M
        35: pulse cannons and other ability sprites
        40: Auton
        50: sniper target
        80: flying safe
        199: shop strobe alarm

    Boss
        -201..-200: core nmy \M
        0: Lead boss
        30-32: nmyLaser
        50: Lead boss attacks
        100: firecracker sparks
        900: ion bomb
    Blackout
        0: blackout vig
        100..106: forge rings & beam
    UI
        -1010: observatory vig
        -999: minimap  \M && timer && stage
            -995: minimap player
        -400: defensiveFX
        -200: guardTxt & weaponNames & observatoryTxt
        -160..-???: safe HUD \M
        -150..-147: blacksmith window \M
            -149: options \M
            -148: payment \M
            -147: coolLine
        -99: vaseGuy text && gold hex prices
        10: room CLEAR anim  \M  &&  UIFrame \M
        12: sniper lines
        15: UI hp and shield bars \M  &&  augWheel
        20: ability CD hex
        30..32: noraa \M
            32: noraa bar \M
        48: remaining rounds \M 48-50
        50: Crosshair; firerate cooldown bars \M; core frame
        52: weapon info display bar
        102: weapon info  \M
            104: display bars\
        180..181: all controls
        200..201: tutorial keys \M
            201: q/e to swap tip
        220: systems tips
        890: warpVig
        900: OPVig
        910: redDmgVig
        990: loading pieces
        995: white flash
        999: OP flash
        1100: paused;


_____________________________________________________________________________________________
PHYSICS LAYERS:

UI: non-game interacty things, visuals only (crosshair, ui, items); mastermind collisions; player minimap obj reveal hallways
Ignore Raycast: custom wall (vase wall), enemy "bumper" layer
TransparentFX: nmy scanner; nmy detection stuff
vase: vase game vases; cng gasExpl


_____________________________________________________________________________________________
OTHER

    textObj is for size ~48 font

    1: end room placed; 2: first borders completed; 3: last borders completed; 4: wait half sec

    stuff that uses HP: baseNmy, core enemy, wall script, chest script, rocket script, chain lightning, dummy script, 

    >>majorAugs array in player script<<
    0: Clutch (<4 hp -> 1 dmg); 1: Lifeline (175% armor on 10% hp threshold); 2: depleted uranium; 3: extra powder; 4: revitalize (% heal at end);
    5: buffer (armor when take dmg); 6: fortified (flat reduction); 7: reactive armor (thorns) 8: accrue; 9: resurgence (heal buff);
    10: p2w; 11: overkill; 12: aimbot; 13: efficiency; 14: siphon; 15: loaded; 16: rifled barrel; 17: full metal jacket 18: Voltaic Armor;
    19: hyper charge; 20: catalyst; 21: dielectric; 22: execute; 23: defensive stance; 24: adaptive armor; 25: bastion; 26: composite armor 27: sabotage
    28: deadly proximity

    buff: full metal jacket,

_____________________________________________________________________________________________
DIFFICULTY RANKING

    Jeep
    Tank G                  
    Rocket Truck G
    Helicopter G
    Buzz Tank

    Tank R
    /Rocket Truck R
    \Helicopter R
    /Turret G

    |Mortar G
    \Taser tank
    
    Enemy barricade placer
    Rocket Turret
    Plane (including plane G for sector 1)
    Drone/UFO
    Turret R
    Mortar R

    ---------------------

    Jeep
    Tank G                  
    Rocket Truck G
    Buzz Tank
    Helicopter G
    Plane G

    Tank R
    Helicopter R
    Rocket Truck R
    Turret G
    Taser tank
    Mortar G

    Rocket Turret G
    Plane R
    Reconnaisance
    Drone/UFO
    Turret R
    Mortar R
    
    Enemy barricade placer
*/