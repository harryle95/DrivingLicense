import { createFileRoute } from '@tanstack/react-router'

import ReactMarkdown from 'react-markdown'
import { useCallback, useState } from 'react'
import type { components } from '@/lib/api/v1'
import { $api, queryClient } from '@/lib/api'

const queryOption = () => $api.queryOptions('get', '/Question')

interface CardProps {
  question: components['schemas']['QuestionGetDTO']
  showQuestion: boolean 
  setShowQuestion: (state: boolean)=>void
}

const Card = (props: CardProps) => {
  const { question, showQuestion, setShowQuestion } = props

  return (
    <button
      className="flex-col border-2 rounded-md p-6 h-[600px] w-[700px]"
      onClick={() => setShowQuestion(!showQuestion)}
    >
      {showQuestion ? (
        <>
          <div>{question.header}</div>
          <div>
            <ReactMarkdown>{question.body}</ReactMarkdown>
          </div>
        </>
      ) : (
        <div>
          <ReactMarkdown>{question.answer}</ReactMarkdown>
        </div>
      )}
    </button>
  )
}

export const Route = createFileRoute('/')({
  component: RouterComponent,
  loader: () => queryClient.ensureQueryData(queryOption()),
})

function RouterComponent() {
  const data = Route.useLoaderData()
  const size = data.length;
  const [index, setIndex] = useState(0);
  const [showQuestion, setShowQuestion] = useState<boolean>(true);

  const setNext = useCallback(()=>{
    setIndex(value=>(value + 1)%size)
    setShowQuestion(true)
  },[size])
  const setPrev = useCallback(()=>{
    const prevIndex = index == 0?size-1:index-1;
    setShowQuestion(true)
    setIndex(prevIndex)
  },[size])

  return (
    <div className="h-full w-full flex">
      <button onClick={setPrev}>Prev</button>
      <Card question={data[index]} showQuestion={showQuestion} setShowQuestion={setShowQuestion}/>
      <button onClick={setNext}>Next</button>
    </div>
  )
}
