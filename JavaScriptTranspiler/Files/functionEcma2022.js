// ==========================================
// AST STRESS TEST: THE FUNCTION
// Requires: allowReturnOutsideFunction: true
// ==========================================
"use strict";

return async function masterFunction(
    { a, b: renamedB = 10 },
    [firstArrayItem, ...restArrayItems],
    standardParam = 42,
    ...restParams
) {

    // 1. Lexical 'this' scoping (Arrow Functions)
    const arrowFunc = (multiplier) => {
        // Arrow functions don't have their own 'this' or 'arguments'
        return this.contextValue * multiplier + arguments.length;
    };

    // 2. Nested Generator Function (State machine)
    function* nestedGenerator() {
        let counter = 0;
        let reset = yield counter++;
        if (reset) {
            // Yield delegation to an iterable (the restParams array)
            yield* restParams;
        }
    }

    // 3. Immediately Invoked Async Arrow Function (IIFE)
    let asyncResult = await (async () => {
        // Mocking an awaitable object to avoid built-in Promises
        let localAwaitable = { then: (cb) => cb(99) };
        return await localAwaitable;
    })();

    // 4. Standard inner closure
    function innerClosure() {
        // Captures variables from the parent function's scope
        return a + renamedB + firstArrayItem + standardParam + asyncResult;
    }

    // 5. Returning the results
    return {
        closureResult: innerClosure(),
        arrowResult: arrowFunc(2),
        generatorObj: nestedGenerator()
    };
};