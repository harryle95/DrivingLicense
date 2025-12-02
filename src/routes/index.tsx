import { createFileRoute, useNavigate } from '@tanstack/react-router'

import ReactMarkdown from 'react-markdown'
import { useEffect, useState } from 'react'
import {
  ArrowLeft,
  ArrowRight,
  NotebookPen,
  RotateCcw,
  Star,
} from 'lucide-react'
import type { components } from '@/lib/api/v1'
import { $api, queryClient } from '@/lib/api'

const queryOption = () => $api.queryOptions('get', '/Question')

interface CardProps {
  question: components['schemas']['QuestionGetDTO']
  showQuestion: boolean
  setShowQuestion: (state: boolean) => void
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
          <div className="font-semibold text-lg">{question.header}</div>
          <div className="flex justify-center items-center">
            <div className="flex-col justify-center items-center p-4">
              <ReactMarkdown>{question.body}</ReactMarkdown>
            </div>
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
  const size = data.length
  const [index, setIndex] = useState(0)
  const [showQuestion, setShowQuestion] = useState<boolean>(true)

  const [reviewQuestion, updateReviewQuestion] = useState<Set<number>>(
    () => new Set(),
  )
  const isReviewQuestion = reviewQuestion.has(data[index].id)
  // Mutation
  const navigate = useNavigate()
  const reviewMutation = $api.useMutation('post', '/Question/Review', {
    onSuccess: () => navigate({ to: '/', reloadDocument: true }),
  })
  const resetMutation = $api.useMutation('post', '/Question/Reset', {
    onSuccess: () => navigate({ to: '/', reloadDocument: true }),
  })

  // Event handler
  const reviewHandler = () => {
    reviewMutation.mutate({ body: [...reviewQuestion] })
  }
  const resetHandler = () => {
    resetMutation.mutate({})
  }
  const addOrRemoveFromReview = () => {
    updateReviewQuestion(prev => {
      const newSet = new Set(prev)
      newSet.has(data[index].id)
        ? newSet.delete(data[index].id)
        : newSet.add(data[index].id)
      return newSet
    })
  }
  const setNext = () => {
    setIndex(value => (value + 1) % size)
    setShowQuestion(true)
  }
  const setPrev = () => {
    setIndex(value => (value == 0 ? size - 1 : value - 1))
    setShowQuestion(true)
  }
  const handleKeyPress = (event: KeyboardEvent) => {
    if (event.key == 'Enter') {
      setShowQuestion(value => !value)
    } else if (event.key == 'ArrowLeft') {
      setPrev()
    } else if (event.key == 'ArrowRight') {
      setNext()
    }
  }
  // Global event listener
  useEffect(() => {
    window.addEventListener('keydown', handleKeyPress)

    return () => {
      window.removeEventListener('keydown', handleKeyPress)
    }
  }, []) // Empty dependency array

  return (
    <div className="flex-col gap-y-4">
      <div className="flex gap-x-4 p-4 justify-center items-center">
        <div>
          {index + 1}/{size}
        </div>
        <div className="flex gap-x-2">
          <Star />
          {reviewQuestion.size}
        </div>
        <button className="flex gap-x-2" onClick={resetHandler}>
          <RotateCcw />
          Reset
        </button>
        <button className="flex gap-x-2" onClick={reviewHandler}>
          <NotebookPen />
          Review
        </button>
      </div>
      <div className="h-full w-full flex justify-center gap-x-4 items-center">
        <button className="p-4" onClick={setPrev}>
          <ArrowLeft />
        </button>
        <div className="relative">
          <button
            onClick={addOrRemoveFromReview}
            className="absolute right-5 top-5 "
          >
            {isReviewQuestion ? <Star className="fill-yellow-300" /> : <Star />}
          </button>
          <Card
            question={data[index]}
            showQuestion={showQuestion}
            setShowQuestion={setShowQuestion}
          />
        </div>
        <button className="p-4" onClick={setNext}>
          <ArrowRight />
        </button>
      </div>
    </div>
  )
}
