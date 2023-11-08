module Car
let drive distance gas =
    let gasPerMile = 0.1
    let gasNeeded = distance * gasPerMile
    let gasLeft = gas - gasNeeded
    gasLeft
