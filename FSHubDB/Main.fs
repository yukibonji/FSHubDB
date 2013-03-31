﻿namespace FSHubDB

module Main =

    open System
    open System.IO
    open System.Timers
    open Types
    open Mongo

    let checkNewQuestions dateTime =
        let path = "Questions.txt"
        let oldQuestions = File.ReadAllLines path
        let questions = Questions.collect()
        let newQuestions = questions |> Array.filter (fun x -> Array.exists (fun y -> y = x.Link) oldQuestions = false)
        let count = newQuestions.Length
        match count with
            | 0 -> ()
            | _ ->
//                Collections.insertQuestions newQuestions
                newQuestions |> Array.iter (fun x -> printfn "%s" x.Link)
                let links = questions |> Array.map (fun x -> x.Link)
                File.WriteAllLines(path, links)
                printfn "%s: %d new question(s)" dateTime count        

    let populateDb _ =
        let now = DateTime.Now.ToString()
        checkNewQuestions now
        
    let timer = new Timer(300000.)
    timer.Enabled <- true
    timer.Elapsed |> Event.add populateDb
    
    populateDb()

    while true do ()