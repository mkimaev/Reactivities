

interface Duck{
    name: string,
    numLegs: number,
    makeSound: (sound: string) => void;
}


const duck1: Duck = {
    name: "duck1",
    numLegs: 2,
    makeSound: (sound: string) => console.log(sound)
}

const duck2: Duck = {
    name: "duck2",
    numLegs: 2,
    makeSound: (sound: string) => console.log(sound)
}

duck1.makeSound("quack");
duck2.makeSound("sound");
duck1.name = '42';

export const ducks = [duck1, duck2]